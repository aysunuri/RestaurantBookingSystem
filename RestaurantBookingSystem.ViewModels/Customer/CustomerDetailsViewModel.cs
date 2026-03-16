using RestaurantBookingSystem.Data.Models.Enums;

namespace RestaurantBookingSystem.ViewModels.Customer
{
    public class CustomerDetailsViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? Email { get; set; }

        public CustomerStatus Status { get; set; }

        public string? Notes { get; set; }

        public int TotalReservations { get; set; }

        public int UpcomingReservations { get; set; }

        public int CompletedReservations { get; set; }

        public DateTime? FirstReservationDate { get; set; }

        public DateTime? LastReservationDate { get; set; }

        public ICollection<CustomerReservationViewModel> ReservationHistory { get; set; } 
            = new List<CustomerReservationViewModel>();
    }
}