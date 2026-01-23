using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.Common.EntityValidation;

namespace RestaurantBookingSystem.Models
{
    public class Table // physical table in the restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(MinTableNumber, MaxTableNumber)]
        public int TableNumber { get; set; }

        [Required]
        [Range(MinSeats,MaxSeats)]
        public int Seats { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; } 
            = new List<Reservation>();
    }
}
