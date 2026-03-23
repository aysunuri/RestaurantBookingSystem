using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;

namespace RestaurantBookingSystem.Data.Repository
{
    public class ReservationRepository : BaseRepository ,IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
        public async Task AddAsync(Reservation reservation)
        {
           await DbContext!.Reservations.AddAsync(reservation);
        }

        public void Delete(Reservation reservation)
        {
            DbContext!.Reservations.Remove(reservation);
        }

        public async Task<IEnumerable<Reservation>> GetAllWithDetailsAsync()
        {
            return await DbContext!.Reservations
              .Include(r => r.Customer)
              .Include(r => r.Table)
              .OrderBy(r => r.Date)
              .ThenBy(r => r.Time)
              .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await DbContext!.Reservations.FindAsync(id);
        }

        public async Task<Reservation?> GetByIdWithCustomerAsync(int id)
        {
            return await DbContext!.Reservations
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Reservation?> GetByIdWithDetailsAsync(int id)
        {
            return await DbContext!.Reservations
               .Include(r => r.Customer)
               .Include(r => r.Table)
               .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetRecentReservationsAsync(int days = 7)
        {
            var cutOffDate = DateTime.Today.AddDays(-days);

            return await DbContext!.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .Where(r => r.Date >= cutOffDate)
                .OrderBy(r => r.Date)
                .ThenBy(r => r.Time)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetTodayReservationsAsync()
        {
            var today = DateTime.Today;

            return await DbContext!.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .Where(r => r.Date.Date == today)
                .OrderBy(r => r.Time)
                .ToListAsync();
        }

        public async Task<bool> IsTableAvailableAsync(int tableId, DateTime date, TimeSpan time, int? ignoreReservationId = null)
        {
            var duration = TimeSpan.FromHours(3);
            var start = time;
            var end = time.Add(duration);

            var reservations = await DbContext!.Reservations
           .Where(r =>
               r.TableId == tableId &&
               r.Date.Date == date.Date &&
               (!ignoreReservationId.HasValue || r.Id != ignoreReservationId.Value))
               .Select(r => r.Time)
               .ToListAsync();

            var hasConflict = reservations.Any(reservationTime =>
               reservationTime < end && reservationTime.Add(duration) > start
             );

            return !hasConflict;
        }

        public void Update(Reservation reservation)
        {
            DbContext!.Reservations.Update(reservation);
        }
        public new async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
