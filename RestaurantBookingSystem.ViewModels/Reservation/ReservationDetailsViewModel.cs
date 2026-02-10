namespace RestaurantBookingSystem.ViewModels.Reservation
{
    public class ReservationDetailsViewModel
    {
        public int Id { get; set; }
        public string Date { get; set; } = null!;
        public string Time { get; set; } = null!;
        public int NumberOfGuests { get; set; } 
        public string? Notes { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public string? CustomerEmail { get; set; } 
        public int TableNumber { get; set; } 
        public int TableSeats { get; set; }


    }
}
