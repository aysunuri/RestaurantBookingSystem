using RestaurantBookingSystem.ViewModels.Event;
using RestaurantBookingSystem.ViewModels.Events;

namespace RestaurantBookingSystem.Services.Contracts
{
    public interface IEventService
    {
        Task<IEnumerable<EventIndexViewModel>> GetAllEventsAsync();
        Task<IEnumerable<EventIndexViewModel>> GetActiveEventsAsync();
        Task<EventDetailsViewModel?> GetEventDetailsAsync(int id);
        Task<EventFormViewModel?> GetEventForEditAsync(int id);
        Task CreateEventAsync(EventFormViewModel model);
        Task<bool> UpdateEventAsync(EventFormViewModel model);
        Task<bool> DeleteEventAsync(int id);
    }
}