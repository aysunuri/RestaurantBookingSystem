using RestaurantBookingSystem.Data.Models.Enums;
namespace RestaurantBookingSystem.ViewModels.Customer
{
    public class CustomerIndexViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? Email { get; set; }

        public CustomerStatus Status { get; set; }

        public int TotalReservations { get; set; }

        public DateTime? LastReservationDate { get; set; }
    }
}
