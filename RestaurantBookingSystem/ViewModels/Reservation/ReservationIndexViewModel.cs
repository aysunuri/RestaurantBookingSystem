namespace RestaurantBookingSystem.ViewModels.Reservation
{
    public class ReservationIndexViewModel
    {
        public int Id { get; set; }
        public string Date { get; set; } = null!;
        public string Time { get; set; } = null!;
        public int NumberOfGuests { get; set; }
        public string CustomerName { get; set; } = null!;
        public int TableNumber { get; set; }
    }
}
