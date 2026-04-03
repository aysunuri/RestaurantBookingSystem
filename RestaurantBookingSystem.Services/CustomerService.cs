using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Customer;
using RestaurantBookingSystem.ViewModels.Shared;

namespace RestaurantBookingSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CustomerIndexViewModel>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return _mapper
               .Map<List<CustomerIndexViewModel>>(customers)
               .OrderBy(c => c.FullName)
               .ToList();
        }

        public async Task<CustomerDetailsViewModel?> GetCustomerDetailsAsync(int id)
        {
            var customer = await _customerRepository.GetByIdWithReservationsAsync(id);

            if (customer == null)
                return null;

            var today = DateTime.Today;

            var result = _mapper.Map<CustomerDetailsViewModel>(customer);

            result.ReservationHistory = customer.Reservations
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
                .ToList();

            return result;
        }
        

        public async Task<CustomerEditViewModel?> GetCustomerForEditAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return null;

            return _mapper.Map<CustomerEditViewModel>(customer);
        }

        public async Task<PagedResult<CustomerIndexViewModel>> GetPagedCustomersAsync(int page, int pageSize, string? searchTerm = null)
        {
            var query = _customerRepository.GetAllWithIncludes().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower().Trim();
                query = query.Where(c =>
                    c.FullName.ToLower().Contains(searchTerm) ||
                    c.PhoneNumber.Contains(searchTerm) ||
                    (c.Email != null && c.Email.ToLower().Contains(searchTerm)));
            }

            var totalItems = await query.CountAsync();
            var customers = await query
                  .OrderBy(c => c.FullName)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();

            return new PagedResult<CustomerIndexViewModel>
            {
                Items = _mapper.Map<List<CustomerIndexViewModel>>(customers),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };
        }

        public async Task<IEnumerable<CustomerIndexViewModel>> SearchCustomersAsync(string searchTerm)
        {
            var customers = await _customerRepository.SearchAsync(searchTerm);

            return _mapper.Map<List<CustomerIndexViewModel>>(customers);
        }

        public async Task<bool> UpdateCustomerStatusAsync(CustomerEditViewModel model)
        {
            var customer = await _customerRepository.GetByIdAsync(model.Id);

            if (customer == null)
                return false;

            customer.Status = model.Status;
            customer.Notes = model.Notes;

            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync();

            return true;
        }

    }
}
