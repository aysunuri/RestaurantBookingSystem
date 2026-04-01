using RestaurantBookingSystem.Data.Models;

namespace RestaurantBookingSystem.Data.Repository.Contracts
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<IEnumerable<Customer>> SearchAsync(string searchTerm);
        IQueryable<Customer> GetAllWithIncludes();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> GetByIdWithReservationsAsync(int id);
        Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
        Task<Customer?> GetByExactMatchAsync(string fullName, string phoneNumber, string? email);
        Task<int> SaveChangesAsync();
        Task AddAsync(Customer customer);
        void Update(Customer customer);
    }
}
