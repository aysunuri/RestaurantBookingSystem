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
                },
                new Reservation
                {
                     Id = 3,
                     Date = new DateTime(2026, 2, 17),
                     Time = new TimeSpan(18, 0, 0),
                     NumberOfGuests = 3,
                     Notes = "Anniversary",
                     CustomerId = 3,
                     TableId = 3
                },
             
                 new Reservation
                 {
                     Id = 4,
                     Date = new DateTime(2026, 2, 18),
                     Time = new TimeSpan(20, 0, 0),
                     NumberOfGuests = 2,
                     Notes = "Date night",
                     CustomerId = 4,
                     TableId = 4
                 },
                 new Reservation
                 {
                     Id = 5,
                     Date = new DateTime(2026, 2, 18),
                     Time = new TimeSpan(13, 0, 0),
                     NumberOfGuests = 5,
                     Notes = "Business lunch",
                     CustomerId = 5,
                     TableId = 3
                 },

                 new Reservation
                 {
                     Id = 6,
                     Date = new DateTime(2026, 2, 19),
                     Time = new TimeSpan(17, 30, 0),
                     NumberOfGuests = 2,
                     Notes = "Early dinner",
                     CustomerId = 6,
                     TableId = 1
                 },
                 new Reservation
                 {
                     Id = 7,
                     Date = new DateTime(2026, 2, 20),
                     Time = new TimeSpan(21, 0, 0),
                     NumberOfGuests = 6,
                     Notes = "Friends gathering",
                     CustomerId = 7,
                     TableId = 5
                 },
                 new Reservation
                 {
                     Id = 8,
                     Date = new DateTime(2026, 2, 21),
                     Time = new TimeSpan(19, 30, 0),
                     NumberOfGuests = 2,
                     Notes = "Valentine's Day",
                     CustomerId = 8,
                     TableId = 4
                 },
                 new Reservation
                 {
                     Id = 9,
                     Date = new DateTime(2026, 2, 22),
                     Time = new TimeSpan(12, 0, 0),
                     NumberOfGuests = 3,
                     Notes = "Casual lunch",
                     CustomerId = 9,
                     TableId = 2
                 },
                 new Reservation
                 {
                     Id = 10,
                     Date = new DateTime(2026, 2, 23),
                     Time = new TimeSpan(18, 45, 0),
                     NumberOfGuests = 4,
                     Notes = "Double date",
                     CustomerId = 10,
                     TableId = 1
                 }


        };
        public void Configure(EntityTypeBuilder<Reservation> entity)
        {
            entity.HasData(Reservations);
        }
    }
}

