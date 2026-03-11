namespace RestaurantBookingSystem.ViewModels.Tables
{
    public class TableDetailsViewModel
    {
        public int Id { get; set; }

        public int TableNumber { get; set; }

        public int Seats { get; set; }
        public int TodayReservationCount { get; set; }
        public int TotalReservations { get; set; }

    }
}
