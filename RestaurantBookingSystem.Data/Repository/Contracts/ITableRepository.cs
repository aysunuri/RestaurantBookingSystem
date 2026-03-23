using RestaurantBookingSystem.Data.Models;
namespace RestaurantBookingSystem.Data.Repository.Contracts
{
    public interface ITableRepository
    {
        Task<IEnumerable<Table>> GetAllAsync();
        Task<Table?> GetByIdAsync(int id);
        Task<bool> TableHasEnoughSeatsAsync(int tableId, int guests);
        Task<bool> TableNumberExistsAsync(int tableNumber, int? excludeId = null);
        Task<bool> HasFutureReservationsAsync(int tableId);
        Task<int> GetTodayReservationCountAsync(int tableId);
        Task<int> GetTotalReservationCountAsync(int tableId);
        Task<int> SaveChangesAsync();

        Task AddAsync(Table table);
        void Update(Table table);
        void Delete(Table table);
    }
}
