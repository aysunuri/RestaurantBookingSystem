using RestaurantBookingSystem.ViewModels;
using RestaurantBookingSystem.ViewModels.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBookingSystem.Services.Contracts
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationIndexViewModel>> GetAllReservationsAsync();
        Task<ReservationDetailsViewModel?> GetReservationDetailsAsync(int id);

        Task<ReservationFormViewModel?> GetReservationFormModelAsync(int id);
        Task AddReservationAsync(ReservationFormViewModel model);

        Task<ReservationFormViewModel?> GetReservationForEditAsync (int id);
        Task<bool> EditReservationAsync (ReservationFormViewModel model);

        Task<bool> DeleteReservationAsync(int id);

        Task<IEnumerable<ReservationIndexViewModel>> GetTodayReservationsAsync();
        Task<IEnumerable<DropDownItemViewModel>> GetTablesDropDownAsync();

        Task<bool> TableHasEnoughSeatsAsync(int tableId, int guests);
        Task<bool> TableIsAvailableAsync (int tableId, DateTime date, TimeSpan time, int? ignoreReservationId = null);

    }
}
