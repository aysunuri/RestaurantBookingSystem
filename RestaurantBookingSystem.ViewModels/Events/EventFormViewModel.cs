using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.GCommon.ValidationConstants;
using static RestaurantBookingSystem.ViewModels.ValidationMessages.EventValidationMessages;

namespace RestaurantBookingSystem.ViewModels.Events
{
    public class EventFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = EventNameIsRequired)]
        [MaxLength(EventNameMaxLength)]
        [Display(Name = "Event Name")]
        public string Name { get; set; } = null!;

        [MaxLength(EventDescriptionMaxLength)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = EventDateIsRequired)]
        [Display(Name = "Event Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Event Image")]
        [MaxLength(ImageUrlMaxLength)]
        [Url(ErrorMessage = ValidUrlIsRequired)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
