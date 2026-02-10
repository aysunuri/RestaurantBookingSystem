using System.ComponentModel.DataAnnotations;


namespace RestaurantBookingSystem.Data.Models
{
    public class Table // physical table in the restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TableNumber { get; set; }

        [Required]
        public int Seats { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; } 
            = new List<Reservation>();
    }
}
