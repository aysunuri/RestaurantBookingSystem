using System.ComponentModel.DataAnnotations;
using static RestaurantBookingSystem.GCommon.ValidationConstants;
using static RestaurantBookingSystem.ViewModels.ValidationMessages.TableValidationMessages;

namespace RestaurantBookingSystem.ViewModels.Tables
{
    public class TableFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = TableNumberRequired)]
        [Range(MinTableNumber, MaxTableNumber, ErrorMessage = TableNumberRange)]
        public int TableNumber { get; set; }

        [Required(ErrorMessage = SeatsRequired)]
        [Range(MinSeats, MaxSeats, ErrorMessage = SeatsRequired)]
        public int Seats { get; set; }
    }
}
