using RestaurantBookingSystem.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.GCommon.ValidationConstants;

namespace RestaurantBookingSystem.Data.Models
{
    public class Customer 
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CustomerFullNameMaxLength)]
        public string FullName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(EmailMaxLength)]
        public string? Email { get; set; }

        public CustomerStatus Status { get; set; } = CustomerStatus.Regular;

        [MaxLength(CustomerNotesMaxLength)]
        public string? Notes { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; } 
            = new List<Reservation>();
    }
}
