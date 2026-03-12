namespace RestaurantBookingSystem.ViewModels.ValidationMessages
{
    public static class TableValidationMessages
    {
        public const string TableNumberRequired = "Table number is required.";
        public const string SeatsRequired= "Number of seats is required.";
        public const string TableNumberRange = "Please enter a value between 1 and 999";
        public const string SeatsRange = "Please enter a value between 1 and 20";

    }
}
