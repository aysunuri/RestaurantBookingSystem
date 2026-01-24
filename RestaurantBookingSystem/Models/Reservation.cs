using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RestaurantBookingSystem.Common.EntityValidation;

namespace RestaurantBookingSystem.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        [Range(MinGuests, MaxGuests)]
        public int NumberOfGuests { get; set; }
        public string? Notes { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Table))]
        public int TableId { get; set; }
        public virtual Table Table { get; set; } = null!;

    }
}
