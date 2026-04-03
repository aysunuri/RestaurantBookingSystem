using AutoMapper;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.ViewModels.Reservation;

namespace RestaurantBookingSystem.MappingProfiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationIndexViewModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.ToString(@"hh\:mm")))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.TableNumber, opt => opt.MapFrom(src => src.Table.TableNumber));

            CreateMap<Reservation, ReservationDetailsViewModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.ToString(@"hh\:mm")))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
                .ForMember(dest => dest.TableNumber, opt => opt.MapFrom(src => src.Table.TableNumber))
                .ForMember(dest => dest.TableSeats, opt => opt.MapFrom(src => src.Table.Seats));

            CreateMap<ReservationFormViewModel, Reservation>();
        }
    }
}