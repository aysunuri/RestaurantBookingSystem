using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantBookingSystem.Data.Models;
using System;


namespace RestaurantBookingSystem.Data.Configurations
{
    internal class RestaurantSettingsConfiguration : IEntityTypeConfiguration<RestaurantSettings>
    {
        private static readonly IEnumerable<RestaurantSettings> restaurantSettings = new List<RestaurantSettings>
        {
           new RestaurantSettings {Id = 1,RestaurantName = "Byte & Bite", OpeningHour = new TimeSpan(10, 0, 0), ClosingHour = new TimeSpan(23, 0, 0) }
        };
        public void Configure(EntityTypeBuilder<RestaurantSettings> entity)
        {
            entity.HasData(restaurantSettings);
        }
    }
}
