using AutoMapper;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Tables;

namespace RestaurantBookingSystem.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public TableService(ITableRepository tableRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TableIndexViewModel>> GetAllTablesAsync()
        {
            var tables = await _tableRepository.GetAllAsync();

            return _mapper.Map<List<TableIndexViewModel>>(tables);
        }
        public async Task<TableDetailsViewModel?> GetTableDetailsAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
                return null;

            int todayReservations = await _tableRepository.GetTodayReservationCountAsync(id);
            int totalReservations = await _tableRepository.GetTotalReservationCountAsync(id);
            int futureReservations = await _tableRepository.GetFutureReservationCountAsync(id);

            var result = _mapper.Map<TableDetailsViewModel>(table);

            result.TodayReservationCount = todayReservations;
            result.TotalReservations = totalReservations;
            result.FutureReservationCount = futureReservations;

            return result;
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

            return _mapper.Map<TableFormViewModel>(table);
        }
        public async Task AddTableAsync(TableFormViewModel model)
        {
            if (await TableNumberExistsAsync(model.TableNumber))
            {
                throw new InvalidOperationException($"Table number {model.TableNumber} already exists.");
            }

            var table = _mapper.Map<Table>(model);

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

            _mapper.Map(model, table);

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
