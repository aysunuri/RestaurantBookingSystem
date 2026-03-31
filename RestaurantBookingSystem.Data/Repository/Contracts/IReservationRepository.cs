using RestaurantBookingSystem.Data.Models;

namespace RestaurantBookingSystem.Data.Repository.Contracts
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllWithDetailsAsync();
        Task<IEnumerable<Reservation>> GetRecentReservationsAsync(int days = 7);
        Task<IEnumerable<Reservation>> GetTodayReservationsAsync();
        Task<Reservation?> GetByIdWithDetailsAsync(int id);
        Task<Reservation?> GetByIdWithCustomerAsync(int id);
        Task<Reservation?> GetByIdAsync(int id);
        Task<bool> IsTableAvailableAsync(int tableId, DateTime date, TimeSpan time, int? ignoreReservationId = null);
        IQueryable<Reservation> GetAllWithIncludes();
        Task AddAsync(Reservation reservation);
        void Update(Reservation reservation);
        void Delete(Reservation reservation);
        Task<int> SaveChangesAsync();
    }
}
