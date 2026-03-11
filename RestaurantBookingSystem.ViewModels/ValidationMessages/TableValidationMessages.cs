namespace RestaurantBookingSystem.ViewModels.ValidationMessages
{
    public static class TableValidationMessages
    {
        public const string TableNumberRequired = "Table number is required";
        public const string SeatsRequired= "Number of seats is required";
        public const string TableNumberRange = "Table number must be between {1} and {2}";
        public const string SeatsRange = "Seats must be between {1} and {2}";

    }
}
