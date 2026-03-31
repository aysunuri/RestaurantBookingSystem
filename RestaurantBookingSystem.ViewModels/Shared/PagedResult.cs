namespace RestaurantBookingSystem.ViewModels.Shared
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
        public int TotalItems { get; set; }  
        public int PageSize { get; set; }     

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
