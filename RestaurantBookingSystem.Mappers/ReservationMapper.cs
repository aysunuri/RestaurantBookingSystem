using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.ViewModels.Reservation;

namespace RestaurantBookingSystem.Mappers
{
    public static class ReservationMapper
    {
        public static ReservationIndexViewModel ToIndexViewModel (Reservation reservation)
        {
            return new ReservationIndexViewModel
            {
                Id = reservation.Id,
                Date = reservation.Date.ToShortDateString(),
                Time = reservation.Time.ToString(@"hh\:mm"),
                NumberOfGuests = reservation.NumberOfGuests,
                CustomerName = reservation.Customer.FullName,
                TableNumber = reservation.Table.TableNumber
            };
        }
        public static ReservationDetailsViewModel ToDetailsViewModel (Reservation reservation)
        {
            return new ReservationDetailsViewModel
            {
                Id = reservation.Id,
                Date = reservation.Date.ToShortDateString(),
                Time = reservation.Time.ToString(@"hh\:mm"),
                NumberOfGuests = reservation.NumberOfGuests,
                Notes = reservation.Notes,
                CustomerName = reservation.Customer.FullName,
                CustomerPhone = reservation.Customer.PhoneNumber,
                CustomerEmail = reservation.Customer.Email,
                TableNumber = reservation.Table.TableNumber,
                TableSeats = reservation.Table.Seats
            };
        }
    }
}
