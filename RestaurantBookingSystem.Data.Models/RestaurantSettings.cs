using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.GCommon.ValidationConstants;

namespace RestaurantBookingSystem.Data.Models
{
    public class RestaurantSettings
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(RestaurantNameMax)]
        public string RestaurantName { get; set; } = null!;
        public TimeSpan OpeningHour { get; set; }
        public TimeSpan ClosingHour { get; set; }
    }
}
