using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;
namespace RestaurantBookingSystem.Data.Repository
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task AddAsync(Customer customer)
        {
            await DbContext!.Customers.AddAsync(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await DbContext!.Customers
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }

        public async Task<Customer?> GetByExactMatchAsync(string fullName, string phoneNumber, string? email)
        {
            return await DbContext!.Customers
               .FirstOrDefaultAsync(c =>
                   c.PhoneNumber == phoneNumber &&
                   c.FullName == fullName &&
                   c.Email == email);
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await DbContext!.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetByIdWithReservationsAsync(int id)
        {
            return await DbContext!.Customers
               .Include(c => c.Reservations)
                   .ThenInclude(r => r.Table)
               .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await DbContext!.Customers
                 .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower().Trim();

            return await DbContext!.Customers
                .Where(c =>
                    c.FullName.ToLower().Contains(searchTerm) ||
                    c.PhoneNumber.Contains(searchTerm) ||
                    (c.Email != null && c.Email.ToLower().Contains(searchTerm)))
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }
        public IQueryable<Customer> GetAllWithIncludes()
        {
            return DbContext!.Customers
                .Include(c => c.Reservations);
        }

        public void Update(Customer customer)
        {
            DbContext!.Customers.Update(customer);
        }
        public new async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
