using RestaurantBookingSystem.Data.Models;

namespace RestaurantBookingSystem.Data.Repository.Contracts
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetActiveEventsAsync();
        Task<Event?> GetByIdAsync(int id);
        Task AddAsync(Event eventEntity);
        void Update(Event eventEntity);
        void Delete(Event eventEntity);
        Task<int> SaveChangesAsync();
    }
}
