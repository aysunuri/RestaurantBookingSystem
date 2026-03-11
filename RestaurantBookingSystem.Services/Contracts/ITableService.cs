
using RestaurantBookingSystem.ViewModels.Tables;

namespace RestaurantBookingSystem.Services.Contracts
{
    public interface ITableService
    {
        Task<IEnumerable<TableIndexViewModel>> GetAllTablesAsync();
        Task<TableDetailsViewModel?> GetTableDetailsAsync(int id);

        Task<TableFormViewModel?> GetTableFormModelAsync(int id);

        Task AddTableAsync(TableFormViewModel model);

        Task<bool> EditTableAsync(TableFormViewModel model);

        Task<bool> DeleteTableAsync(int id);

        Task<bool> TableNumberExistsAsync(int tableNumber, int? excludeId = null);
    }
}
