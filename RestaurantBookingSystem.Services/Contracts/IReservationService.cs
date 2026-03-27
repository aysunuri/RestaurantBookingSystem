using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.ViewModels;
using RestaurantBookingSystem.ViewModels.Reservation;
using RestaurantBookingSystem.ViewModels.Shared;

namespace RestaurantBookingSystem.Services.Contracts
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationIndexViewModel>> GetAllReservationsAsync(bool includeOld = false);
        Task<ReservationDetailsViewModel?> GetReservationDetailsAsync(int id);

        Task<ReservationFormViewModel?> GetReservationFormModelAsync(int id);
        Task AddReservationAsync(ReservationFormViewModel model);

        Task<ReservationFormViewModel?> GetReservationForEditAsync (int id);
        Task<bool> EditReservationAsync (ReservationFormViewModel model);

        Task<bool> DeleteReservationAsync(int id);

        Task<IEnumerable<ReservationIndexViewModel>> GetTodayReservationsAsync();
        Task<IEnumerable<DropDownItemViewModel>> GetTablesDropDownAsync();
        Task<PagedResult<ReservationIndexViewModel>> GetPagedReservationsAsync(int page, int pageSize, bool showAll);


        bool IsValidReservationDateTime(DateTime date, TimeSpan time);
        Task<bool> IsWithinOperatingHoursAsync(TimeSpan time);
        Task<bool> TableHasEnoughSeatsAsync(int tableId, int guests);
        Task<bool> TableIsAvailableAsync (int tableId, DateTime date, TimeSpan time, int? ignoreReservationId = null);

    }
}
