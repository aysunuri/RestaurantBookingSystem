using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantBookingSystem.Data.Models;

namespace RestaurantBookingSystem.Data.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        private static readonly IEnumerable<Reservation> Reservations = new List<Reservation>
        {
                new Reservation
                {
                    Id = 1,
                    Date = new DateTime(2026, 1, 30),
                    Time = new TimeSpan(19, 0, 0),
                    NumberOfGuests = 2,
                    Notes = "Birthday dinner",
                    CustomerId = 1,
                    TableId = 2
                },
                new Reservation
                {
                    Id = 2,
                    Date = new DateTime(2026, 2, 1),
                    Time = new TimeSpan(12, 30, 0),
                    NumberOfGuests = 4,
                    Notes = "Family lunch",
                    CustomerId = 2,
                    TableId = 1
                }
        };
        public void Configure(EntityTypeBuilder<Reservation> entity)
        {
            entity.HasData(Reservations);
        }
    }
}
