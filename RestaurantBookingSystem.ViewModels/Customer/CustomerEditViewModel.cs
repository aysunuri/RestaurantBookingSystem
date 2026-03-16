using System.ComponentModel.DataAnnotations;
using RestaurantBookingSystem.Data.Models.Enums;
using static RestaurantBookingSystem.ViewModels.ValidationMessages.CustomerValidationMessages;

namespace RestaurantBookingSystem.ViewModels.Customer
{
    public class CustomerEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        public string? Email { get; set; }

        [Required]
        [Display(Name = "Customer Status")]
        public CustomerStatus Status { get; set; }

        [MaxLength(500, ErrorMessage = CustomerNotesLengthMessage)]
        [Display(Name = "Staff Notes")]
        public string? Notes { get; set; }
    }
}