using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.Common.EntityValidation;

namespace RestaurantBookingSystem.Models
{
    public class Customer //represents the person who makes the reservation 
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(CustomerFullNameMinLength)]
        [MaxLength(CustomerFullNameMaxLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [EmailAddress]
        public string? Email { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; } 
            = new List<Reservation>();
    }
}
