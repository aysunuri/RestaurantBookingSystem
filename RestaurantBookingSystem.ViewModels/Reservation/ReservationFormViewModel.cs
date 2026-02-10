using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.GCommon.ValidationConstants;
using static RestaurantBookingSystem.ViewModels.ValidationMessages.ReservationValidationMessages;

namespace RestaurantBookingSystem.ViewModels.Reservation
{
    public class ReservationFormViewModel
    {
        public int? Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        [Range(MinGuests, MaxGuests)]
        public int NumberOfGuests { get; set; }

        [MaxLength(NotesMaxLength)]
        public string? Notes { get; set; }

        [Required]
        [MinLength(CustomerFullNameMinLength, ErrorMessage = NameTooShort)]
        [MaxLength(CustomerFullNameMaxLength, ErrorMessage = NameTooLong)]
        public string CustomerName { get; set; } = null!;

        [Required]
        [RegularExpression(PhoneValidationRegex, ErrorMessage =InvalidPhone)]
        public string CustomerPhone { get; set; } = null!;

        [EmailAddress(ErrorMessage = InvalidEmail)]
        [MaxLength(EmailMaxLength)]
        public string? CustomerEmail { get; set; }

        [Required]
        public int TableId { get; set; }

        public IEnumerable<DropDownItemViewModel>? Tables { get; set; }

    }
}
