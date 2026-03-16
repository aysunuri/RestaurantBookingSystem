namespace RestaurantBookingSystem.ViewModels.Customer
{
    public class CustomerReservationViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public int NumberOfGuests { get; set; }

        public int TableNumber { get; set; }

        public string? Notes { get; set; }

        public bool IsPast => Date < DateTime.Today;
    }
}