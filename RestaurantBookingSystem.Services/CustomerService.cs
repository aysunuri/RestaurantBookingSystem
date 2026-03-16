using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Customer;

namespace RestaurantBookingSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CustomerIndexViewModel>> GetAllCustomersAsync()
        {
            return await _context.Customers
               .Select(c => new CustomerIndexViewModel
               {
                   Id = c.Id,
                   FullName = c.FullName,
                   PhoneNumber = c.PhoneNumber,
                   Email = c.Email,
                   Status = c.Status,
                   TotalReservations = c.Reservations.Count,
                   LastReservationDate = c.Reservations
                       .OrderByDescending(r => r.Date)
                       .Select(r => (DateTime?)r.Date)
                       .FirstOrDefault()
               })
               .OrderBy(c => c.FullName)
               .ToListAsync();
        }

        public async Task<CustomerDetailsViewModel?> GetCustomerDetailsAsync(int id)
        {
            var customer = await _context.Customers
                 .Include(c => c.Reservations)
                     .ThenInclude(r => r.Table)
                 .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return null;

            var today = DateTime.Today;

            return new CustomerDetailsViewModel
            {
                Id = customer.Id,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Status = customer.Status,
                Notes = customer.Notes,
                TotalReservations = customer.Reservations.Count,
                UpcomingReservations = customer.Reservations.Count(r => r.Date >= today),
                CompletedReservations = customer.Reservations.Count(r => r.Date < today),
                FirstReservationDate = customer.Reservations
                    .OrderBy(r => r.Date)
                    .Select(r => (DateTime?)r.Date)
                    .FirstOrDefault(),
                LastReservationDate = customer.Reservations
                    .OrderByDescending(r => r.Date)
                    .Select(r => (DateTime?)r.Date)
                    .FirstOrDefault(),
                ReservationHistory = customer.Reservations
                    .OrderByDescending(r => r.Date)
                    .ThenByDescending(r => r.Time)
                    .Select(r => new CustomerReservationViewModel
                    {
                        Id = r.Id,
                        Date = r.Date,
                        Time = r.Time,
                        NumberOfGuests = r.NumberOfGuests,
                        TableNumber = r.Table.TableNumber,
                        Notes = r.Notes
                    })
                    .ToList()
            };
            }

        public async Task<CustomerEditViewModel?> GetCustomerForEditAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return null;

            return new CustomerEditViewModel
            {
                Id = customer.Id,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Status = customer.Status,
                Notes = customer.Notes
            };
        }

        public async Task<IEnumerable<CustomerIndexViewModel>> SearchCustomersAsync(string searchTerm)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(c =>
                    c.FullName.ToLower().Contains(searchTerm) ||
                    c.PhoneNumber.Contains(searchTerm) ||
                    (c.Email != null && c.Email.ToLower().Contains(searchTerm)));
            }

            return await query
                .Select(c => new CustomerIndexViewModel
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    Status = c.Status,
                    TotalReservations = c.Reservations.Count,
                    LastReservationDate = c.Reservations
                        .OrderByDescending(r => r.Date)
                        .Select(r => (DateTime?)r.Date)
                        .FirstOrDefault()
                })
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }

        public async Task<bool> UpdateCustomerStatusAsync(CustomerEditViewModel model)
        {
            var customer = await _context.Customers.FindAsync(model.Id);

            if (customer == null)
                return false;

            customer.Status = model.Status;
            customer.Notes = model.Notes;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
