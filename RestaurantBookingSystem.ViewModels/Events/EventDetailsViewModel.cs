namespace RestaurantBookingSystem.ViewModels.Events
{
    public class EventDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}