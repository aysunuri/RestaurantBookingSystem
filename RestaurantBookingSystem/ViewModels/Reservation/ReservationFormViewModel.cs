using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.Common.ValidationConstants;

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
        [MinLength(CustomerFullNameMinLength)]
        [MaxLength(CustomerFullNameMaxLength)]
        public string CustomerName { get; set; } = null!;

        [Required]
        [RegularExpression(PhoneValidationRegex)]
        public string CustomerPhone { get; set; } = null!;

        [EmailAddress]
        [MaxLength(EmailMaxLength)]
        public string? CustomerEmail { get; set; }

        [Required]
        public int TableId { get; set; }

        public IEnumerable<SelectListItem>? Tables { get; set; }

    }
}
