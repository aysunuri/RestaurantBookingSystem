using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.GCommon.ValidationConstants;

namespace RestaurantBookingSystem.Data.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(EventNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(EventDescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
