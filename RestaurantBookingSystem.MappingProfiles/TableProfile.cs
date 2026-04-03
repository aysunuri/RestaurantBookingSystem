using AutoMapper;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.ViewModels.Tables;

namespace RestaurantBookingSystem.MappingProfiles
{
    public class TableProfile : Profile
    {
        public TableProfile()
        {
            CreateMap<Table, TableIndexViewModel>();
            CreateMap<Table, TableDetailsViewModel>();
            CreateMap<Table, TableFormViewModel>();
            CreateMap<TableFormViewModel, Table>();
        }
    }
}