using AutoMapper;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.ViewModels.Events;

namespace RestaurantBookingSystem.MappingProfiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventIndexViewModel>();
            CreateMap<Event, EventDetailsViewModel>();
            CreateMap<Event, EventFormViewModel>();
            CreateMap<EventFormViewModel, Event>();
        }
    }
}