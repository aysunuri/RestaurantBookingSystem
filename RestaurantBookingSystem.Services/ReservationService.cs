using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels;
using RestaurantBookingSystem.ViewModels.Reservation;

namespace RestaurantBookingSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;
        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ReservationIndexViewModel>> GetAllReservationsAsync()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .ToListAsync();

            return  reservations.Select(r => new ReservationIndexViewModel
            {
                Id = r.Id,
                Date = r.Date.ToShortDateString(),
                Time = r.Time.ToString(@"hh\:mm"),
                NumberOfGuests = r.NumberOfGuests,
                CustomerName = r.Customer.FullName,
                TableNumber = r.Table.TableNumber
            })
            .ToList();

        }
        public async Task<ReservationDetailsViewModel?> GetReservationDetailsAsync(int id)
        {
            var reservation = await _context.Reservations
               .Include(r => r.Customer)
               .Include(r => r.Table)
               .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return null;
            }

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
        public async Task<ReservationFormViewModel?> GetReservationFormModelAsync(int id)
        {
            var tables = await GetTablesDropDownAsync();

            if (id == 0)
            {
                return new ReservationFormViewModel
                {
                    Date = DateTime.Today,
                    Tables = tables
                };
            }

          var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);

            if(reservation == null)
               return null;
            

            return new ReservationFormViewModel
            {
                Id = reservation.Id,
                Date = reservation.Date,
                Time = reservation.Time,
                NumberOfGuests = reservation.NumberOfGuests,
                Notes = reservation.Notes,

                CustomerName = reservation.Customer.FullName,
                CustomerPhone = reservation.Customer.PhoneNumber,
                CustomerEmail = reservation.Customer.Email,

                TableId = reservation.Table.Id,
                Tables = tables
            };
        }

        public async Task AddReservationAsync(ReservationFormViewModel model)
        {
            if(!await TableHasEnoughSeatsAsync(model.TableId, model.NumberOfGuests))
            {
                throw new InvalidOperationException("The selected table doesn't have enough seats for the number of guests.");
            }
            if (!await TableIsAvailableAsync(model.TableId, model.Date, model.Time))
            {
                throw new InvalidOperationException("The selected table is already booked for this time slot.");
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == model.CustomerPhone);

            if (customer == null)
            {
                customer = new Customer
                {
                    FullName = model.CustomerName,
                    PhoneNumber = model.CustomerPhone,
                    Email = model.CustomerEmail
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var reservation = new Reservation
            {
                Date = model.Date,
                Time = model.Time,
                NumberOfGuests = model.NumberOfGuests,
                Notes = model.Notes,
                CustomerId = customer.Id,
                TableId = model.TableId
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DropDownItemViewModel>> GetTablesDropDownAsync()
        {
            return await _context.Tables
             .Select(t => new DropDownItemViewModel
             {
                 Value = t.Id,
                 Text = $"Table {t.TableNumber} - {t.Seats} seats"
             })
            .ToListAsync();
        }
        public async Task<ReservationFormViewModel?> GetReservationForEditAsync(int id)
        {
            var reservation = await _context.Reservations
               .Include(r => r.Customer)
               .Include(r => r.Table)
               .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return null;

            return new ReservationFormViewModel
            {
                Id = reservation.Id,
                Date = reservation.Date,
                Time = reservation.Time,
                NumberOfGuests = reservation.NumberOfGuests,
                Notes = reservation.Notes,
                CustomerName = reservation.Customer.FullName,
                CustomerPhone = reservation.Customer.PhoneNumber,
                CustomerEmail = reservation.Customer.Email,
                TableId = reservation.Table.Id,
                Tables = await GetTablesDropDownAsync(),
            };
        }

        public async Task<bool> EditReservationAsync(ReservationFormViewModel model)
        {
         
            var reservation = await _context.Reservations
               .Include(r => r.Customer)
               .FirstOrDefaultAsync(r => r.Id == model.Id);

            if (reservation == null)
                return false;

            if (!await TableHasEnoughSeatsAsync(model.TableId, model.NumberOfGuests))
            {
                throw new InvalidOperationException("The selected table doesn't have enough seats for the number of guests.");
            }
            if (!await TableIsAvailableAsync(model.TableId, model.Date, model.Time, model.Id))
            {
                throw new InvalidOperationException("The selected table is already booked for this time slot.");
            }

            bool customerChanged =
                 reservation.Customer.FullName != model.CustomerName ||
                 reservation.Customer.PhoneNumber != model.CustomerPhone ||
                 reservation.Customer.Email != model.CustomerEmail;

            if (customerChanged)
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.PhoneNumber == model.CustomerPhone);
                if (existingCustomer != null)
                {
                    reservation.CustomerId = existingCustomer.Id;
                }
                else
                {
                    var newCustomer = new Customer
                    {
                        FullName = model.CustomerName,
                        PhoneNumber = model.CustomerPhone,
                        Email = model.CustomerEmail,
                    };
                    _context.Customers.Add(newCustomer);
                    await _context.SaveChangesAsync();
                    reservation.CustomerId = newCustomer.Id;
                }
            }

            reservation.Date = model.Date;
            reservation.Time = model.Time;
            reservation.NumberOfGuests = model.NumberOfGuests;
            reservation.Notes = model.Notes;
            reservation.TableId = model.TableId;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteReservationAsync(int id)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<ReservationIndexViewModel>> GetTodayReservationsAsync()
        {
            var today = DateTime.Today;

            return await _context.Reservations
                .Where(r => r.Date.Date == today)
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .Select(r => new ReservationIndexViewModel
                {
                    Id = r.Id,
                    Date = r.Date.ToShortDateString(),
                    Time = r.Time.ToString(@"hh\:mm"),
                    NumberOfGuests = r.NumberOfGuests,
                    CustomerName = r.Customer.FullName,
                    TableNumber = r.Table.TableNumber
                })
                .ToListAsync();
        }

        public async Task<bool> TableHasEnoughSeatsAsync(int tableId, int guests)
        {
           var table = await _context.Tables
                .FirstOrDefaultAsync(t => t.Id == tableId);

            if (table == null)
                return false;

            return table.Seats >= guests;
        }

        public async Task<bool> TableIsAvailableAsync(int tableId, DateTime date, TimeSpan time, int? ignoreReservationId = null)
        {
            var duration = TimeSpan.FromHours(3);
            var start = time;
            var end = time.Add(duration);

            return !await _context.Reservations
           .AnyAsync(r =>
               r.TableId == tableId &&
               r.Date.Date == date.Date &&
               (!ignoreReservationId.HasValue || r.Id != ignoreReservationId.Value) &&
               // overlap check
               r.Time < end &&
               r.Time.Add(duration) > start
           );
        }

    }
}
