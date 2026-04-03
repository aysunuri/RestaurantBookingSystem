using AutoMapper;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Events;

namespace RestaurantBookingSystem.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventIndexViewModel>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllAsync();

            return _mapper.Map<List<EventIndexViewModel>>(events);
        }

        public async Task<IEnumerable<EventIndexViewModel>> GetActiveEventsAsync()
        {
            var events = await _eventRepository.GetActiveEventsAsync();

            return _mapper.Map<List<EventIndexViewModel>>(events);
        }

        public async Task<EventDetailsViewModel?> GetEventDetailsAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);

            if (eventEntity == null)
                return null;

            return _mapper.Map<EventDetailsViewModel>(eventEntity);
        }

        public async Task<EventFormViewModel?> GetEventForEditAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);

            if (eventEntity == null)
                return null;

            return _mapper.Map<EventFormViewModel>(eventEntity);
        }

        public async Task CreateEventAsync(EventFormViewModel model)
        {
            var eventEntity = _mapper.Map<Event>(model);

            await _eventRepository.AddAsync(eventEntity);
            await _eventRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateEventAsync(EventFormViewModel model)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(model.Id!.Value);

            if (eventEntity == null)
                return false;

            _mapper.Map(model, eventEntity);

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