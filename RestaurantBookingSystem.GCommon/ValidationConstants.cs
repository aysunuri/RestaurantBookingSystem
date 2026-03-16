namespace RestaurantBookingSystem.GCommon
{
    public static class ValidationConstants
    {
        public const int CustomerFullNameMinLength = 5;
        public const int CustomerFullNameMaxLength = 100;

        public const int MinTableNumber = 1;
        public const int MaxTableNumber = 999;

        public const int MinSeats = 1;
        public const int MaxSeats = 20;

        public const int NotesMaxLength = 300;
        public const int CustomerNotesMaxLength = 500;

        public const int MinGuests = 1;
        public const int MaxGuests = 20;

        public const int EmailMaxLength = 100;
        public const string PhoneValidationRegex = @"^\+?[0-9]{8,15}$";
    }
}
