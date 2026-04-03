using AutoMapper;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.ViewModels.Customer;

namespace RestaurantBookingSystem.MappingProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Customer -> CustomerIndexViewModel
            CreateMap<Customer, CustomerIndexViewModel>()
                .ForMember(dest => dest.TotalReservations, opt => opt.MapFrom(src => src.Reservations.Count))
                .ForMember(dest => dest.LastReservationDate, opt => opt.MapFrom(src =>
                    src.Reservations.OrderByDescending(r => r.Date).Select(r => (DateTime?)r.Date).FirstOrDefault()));

            // Customer -> CustomerDetailsViewModel
            CreateMap<Customer, CustomerDetailsViewModel>()
                .ForMember(dest => dest.TotalReservations, opt => opt.MapFrom(src => src.Reservations.Count))
                .ForMember(dest => dest.UpcomingReservations, opt => opt.MapFrom(src =>
                    src.Reservations.Count(r => r.Date >= DateTime.Today)))
                .ForMember(dest => dest.CompletedReservations, opt => opt.MapFrom(src =>
                    src.Reservations.Count(r => r.Date < DateTime.Today)))
                .ForMember(dest => dest.FirstReservationDate, opt => opt.MapFrom(src =>
                    src.Reservations.OrderBy(r => r.Date).Select(r => (DateTime?)r.Date).FirstOrDefault()))
                .ForMember(dest => dest.LastReservationDate, opt => opt.MapFrom(src =>
                    src.Reservations.OrderByDescending(r => r.Date).Select(r => (DateTime?)r.Date).FirstOrDefault()));

            // Customer -> CustomerEditViewModel
            CreateMap<Customer, CustomerEditViewModel>();

            // CustomerEditViewModel -> Customer
            CreateMap<CustomerEditViewModel, Customer>();
        }
    }
}