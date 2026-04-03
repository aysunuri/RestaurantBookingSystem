using AutoMapper;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.ViewModels.Settings;

namespace RestaurantBookingSystem.MappingProfiles
{
    public class SettingsProfile : Profile
    {
        public SettingsProfile()
        {
            CreateMap<RestaurantSettings, SettingsViewModel>();
            CreateMap<SettingsViewModel, RestaurantSettings>();
        }
    }
}