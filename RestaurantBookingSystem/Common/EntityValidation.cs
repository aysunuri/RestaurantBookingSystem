namespace RestaurantBookingSystem.Common
{
    public static class EntityValidation
    {
        public const int CustomerFullNameMinLength = 5;
        public const int CustomerFullNameMaxLength = 100;

        public const int MinTableNumber = 1;
        public const int MaxTableNumber = 50;

        public const int MinSeats = 1;
        public const int MaxSeats = 20;

        public const int MinGuests = 1;
        public const int MaxGuests = 20;

        public const int EmailMaxLength = 100;
        public const string PhoneValidationRegex = @"^\+?[0-9]{8,15}$";
    }
}
