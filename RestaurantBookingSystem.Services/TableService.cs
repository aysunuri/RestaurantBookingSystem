using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Tables;

namespace RestaurantBookingSystem.Services
{
    public class TableService : ITableService
    {
        private readonly ApplicationDbContext _context;

        public TableService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TableIndexViewModel>> GetAllTablesAsync()
        {
            return await _context.Tables
           .OrderBy(t => t.TableNumber)
           .Select(t => new TableIndexViewModel
           {
               Id = t.Id,
               TableNumber = t.TableNumber,
               Seats = t.Seats,
           })
           .ToListAsync();
        }
        public async Task<TableDetailsViewModel?> GetTableDetailsAsync(int id)
        {
            var table = await _context.Tables.FindAsync(id);

            if (table == null)
                return null;

            int todayReservations = await _context.Reservations
                .CountAsync(r => r.TableId == id && r.Date.Date == DateTime.Today);

            int totalReservations = await _context.Reservations
                .CountAsync(r => r.TableId == id);

            return new TableDetailsViewModel
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Seats = table.Seats,
                TodayReservationCount = todayReservations,
                TotalReservations = totalReservations
            };
        }
        public async Task<TableFormViewModel?> GetTableFormModelAsync(int id)
        {
            if (id == 0)
            {
                return new TableFormViewModel();
            }

            var table = await _context.Tables.FindAsync(id);

            if (table == null)
                return null;

            return new TableFormViewModel
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Seats = table.Seats
            };
        }
        public async Task AddTableAsync(TableFormViewModel model)
        {
            if (await TableNumberExistsAsync(model.TableNumber))
            {
                throw new InvalidOperationException($"Table number {model.TableNumber} already exists.");
            }

            var table = new Table
            {
                TableNumber = model.TableNumber,
                Seats = model.Seats
            };

            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> EditTableAsync(TableFormViewModel model)
        {
            var table = await _context.Tables.FindAsync(model.Id);

            if (table == null)
                return false;

            if (await TableNumberExistsAsync(model.TableNumber, model.Id))
            {
                throw new InvalidOperationException($"Table number {model.TableNumber} already exists.");
            }

            table.TableNumber = model.TableNumber;
            table.Seats = model.Seats;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteTableAsync(int id)
        {
            var table = await _context.Tables.FindAsync(id);

            if (table == null)
                return false;

            var hasReservations = await _context.Reservations
                .AnyAsync(r => r.TableId == id);

            if (hasReservations)
            {
                throw new InvalidOperationException("Cannot delete a table that has reservations. Please reassign or delete the reservations first.");
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TableNumberExistsAsync(int tableNumber, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _context.Tables
                    .AnyAsync(t => t.TableNumber == tableNumber && t.Id != excludeId.Value);
            }

            return await _context.Tables
                .AnyAsync(t => t.TableNumber == tableNumber);
        }
    }
}
