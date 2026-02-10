using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBookingSystem.ViewModels.ValidationMessages
{
    public static class ReservationValidationMessages
    {
        public const string NameTooShort = "Name is too short.";
        public const string NameTooLong = "Name is too long.";
        public const string InvalidPhone = "Please enter a valid phone number.";
        public const string InvalidEmail = "Please enter a valid email address.";
    }
}
