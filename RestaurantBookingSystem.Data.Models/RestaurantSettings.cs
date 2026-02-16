
namespace RestaurantBookingSystem.Data.Models
{
    public class RestaurantSettings
    {
        public int Id { get; set; }
        public TimeSpan OpeningHour { get; set; }
        public TimeSpan ClosingHour { get; set; }
    }
}
