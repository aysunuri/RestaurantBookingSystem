using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Event;
using RestaurantBookingSystem.ViewModels.Events;

namespace RestaurantBookingSystem.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<EventIndexViewModel>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllAsync();

            return events.Select(e => new EventIndexViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                ImageUrl = e.ImageUrl,
                IsActive = e.IsActive
            }).ToList();
        }

        public async Task<IEnumerable<EventIndexViewModel>> GetActiveEventsAsync()
        {
            var events = await _eventRepository.GetActiveEventsAsync();

            return events.Select(e => new EventIndexViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                ImageUrl = e.ImageUrl,
                IsActive = e.IsActive
            }).ToList();
        }

        public async Task<EventDetailsViewModel?> GetEventDetailsAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);

            if (eventEntity == null)
                return null;

            return new EventDetailsViewModel
            {
                Id = eventEntity.Id,
                Name = eventEntity.Name,
                Description = eventEntity.Description,
                Date = eventEntity.Date,
                ImageUrl = eventEntity.ImageUrl,
                IsActive = eventEntity.IsActive
            };
        }

        public async Task<EventFormViewModel?> GetEventForEditAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);

            if (eventEntity == null)
                return null;

            return new EventFormViewModel
            {
                Id = eventEntity.Id,
                Name = eventEntity.Name,
                Description = eventEntity.Description,
                Date = eventEntity.Date,
                ImageUrl = eventEntity.ImageUrl,
                IsActive = eventEntity.IsActive
            };
        }

        public async Task CreateEventAsync(EventFormViewModel model)
        {
            var eventEntity = new Event
            {
                Name = model.Name,
                Description = model.Description,
                Date = model.Date,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive
            };

            await _eventRepository.AddAsync(eventEntity);
            await _eventRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateEventAsync(EventFormViewModel model)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(model.Id!.Value);

            if (eventEntity == null)
                return false;

            eventEntity.Name = model.Name;
            eventEntity.Description = model.Description;
            eventEntity.Date = model.Date;
            eventEntity.ImageUrl = model.ImageUrl;
            eventEntity.IsActive = model.IsActive;

            _eventRepository.Update(eventEntity);
            await _eventRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);

            if (eventEntity == null)
                return false;

            _eventRepository.Delete(eventEntity);
            await _eventRepository.SaveChangesAsync();

            return true;
        }
    }
}