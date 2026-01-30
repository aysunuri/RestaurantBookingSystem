using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.Common.ValidationConstants;

namespace RestaurantBookingSystem.Models
{
    public class Customer //represents the person who makes the reservation 
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CustomerFullNameMaxLength)]
        public string FullName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(EmailMaxLength)]
        public string? Email { get; set; } = null!;

        public virtual ICollection<Reservation> Reservations { get; set; } 
            = new List<Reservation>();
    }
}
