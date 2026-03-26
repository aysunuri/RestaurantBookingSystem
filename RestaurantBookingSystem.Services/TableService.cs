using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Tables;

namespace RestaurantBookingSystem.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;

        public TableService(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public async Task<IEnumerable<TableIndexViewModel>> GetAllTablesAsync()
        {
            var tables = await _tableRepository.GetAllAsync();

            return tables.Select(t => new TableIndexViewModel
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                Seats = t.Seats
            }).ToList();
        }
        public async Task<TableDetailsViewModel?> GetTableDetailsAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
                return null;

            int todayReservations = await _tableRepository.GetTodayReservationCountAsync(id);
            int totalReservations = await _tableRepository.GetTotalReservationCountAsync(id);
            int futureReservations = await _tableRepository.GetFutureReservationCountAsync(id);

            return new TableDetailsViewModel
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Seats = table.Seats,
                TodayReservationCount = todayReservations,
                TotalReservations = totalReservations,
                FutureReservationCount = futureReservations
            };
        }
        public async Task<TableFormViewModel?> GetTableFormModelAsync(int id)
        {
            if (id == 0)
            {
                return new TableFormViewModel();
            }

            var table = await _tableRepository.GetByIdAsync(id);

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

            await _tableRepository.AddAsync(table);
            await _tableRepository.SaveChangesAsync();
        }


        public async Task<bool> EditTableAsync(TableFormViewModel model)
        {
            var table = await _tableRepository.GetByIdAsync(model.Id!.Value);

            if (table == null)
                return false;

            if (await TableNumberExistsAsync(model.TableNumber, model.Id))
            {
                throw new InvalidOperationException($"Table number {model.TableNumber} already exists.");
            }

            table.TableNumber = model.TableNumber;
            table.Seats = model.Seats;

            _tableRepository.Update(table);
            await _tableRepository.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteTableAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
                return false;

            var hasReservations = await _tableRepository.HasFutureReservationsAsync(id);

            if (hasReservations)
            {
                throw new InvalidOperationException("Cannot delete a table with reservation history. Historical data must be preserved.");
            }

            _tableRepository.Delete(table);
            await _tableRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TableNumberExistsAsync(int tableNumber, int? excludeId = null)
        {
            return await _tableRepository.TableNumberExistsAsync(tableNumber, excludeId);
        }
    }
}
