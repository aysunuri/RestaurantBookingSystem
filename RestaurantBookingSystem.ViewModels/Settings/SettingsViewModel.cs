using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.GCommon.ValidationConstants;
using static RestaurantBookingSystem.ViewModels.ValidationMessages.SettingsValidationMessages;

namespace RestaurantBookingSystem.ViewModels.Settings
{
    public class SettingsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = RestaurantNameRequired)]
        [MaxLength(RestaurantNameMax, ErrorMessage = RestaurantNameMaxLength)]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; } = null!;

        [Required(ErrorMessage = OpeningHourRequired)]
        [Display(Name = "Opening Hour")]
        public TimeSpan OpeningHour { get; set; }

        [Required(ErrorMessage = ClosingHourRequired)]
        [Display(Name = "Closing Hour")]
        public TimeSpan ClosingHour { get; set; }
    }
}