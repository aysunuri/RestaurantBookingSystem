using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Data.Repository;

namespace RestaurantBookingSystem.Data.Repositories
{
    public class TableRepository : BaseRepository, ITableRepository
    {
        public TableRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            return await DbContext!.Tables
                .OrderBy(t => t.TableNumber)
                .ToListAsync();
        }

        public async Task<Table?> GetByIdAsync(int id)
        {
            return await DbContext!.Tables.FindAsync(id);
        }

        public async Task<bool> TableHasEnoughSeatsAsync(int tableId, int guests)
        {
            var table = await DbContext!.Tables
                .FirstOrDefaultAsync(t => t.Id == tableId);

            if (table == null)
                return false;

            return table.Seats >= guests;
        }

        public async Task<bool> TableNumberExistsAsync(int tableNumber, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await DbContext!.Tables
                    .AnyAsync(t => t.TableNumber == tableNumber && t.Id != excludeId.Value);
            }

            return await DbContext!.Tables
                .AnyAsync(t => t.TableNumber == tableNumber);
        }

        public async Task<bool> HasFutureReservationsAsync(int tableId)
        {
            return await DbContext!.Reservations
                .AnyAsync(r => r.TableId == tableId && r.Date >= DateTime.Today);
        }
        public async Task<int> GetFutureReservationCountAsync(int tableId)
        {
            return await DbContext!.Reservations
                .CountAsync(r => r.TableId == tableId && r.Date >= DateTime.Today);
        }

        public async Task<int> GetTodayReservationCountAsync(int tableId)
        {
            return await DbContext!.Reservations
                .CountAsync(r => r.TableId == tableId && r.Date.Date == DateTime.Today);
        }

        public async Task<int> GetTotalReservationCountAsync(int tableId)
        {
            return await DbContext!.Reservations
                .CountAsync(r => r.TableId == tableId);
        }

        public async Task AddAsync(Table table)
        {
            await DbContext!.Tables.AddAsync(table);
        }

        public void Update(Table table)
        {
            DbContext!.Tables.Update(table);
        }

        public void Delete(Table table)
        {
            DbContext!.Tables.Remove(table);
        }
        public new async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}