using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Models.Enums;
using RestaurantBookingSystem.Mappers;
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
        public async Task<IEnumerable<ReservationIndexViewModel>> GetAllReservationsAsync(bool includeOld = false)
        {
            var query = _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .AsQueryable();
            if (!includeOld)
            {
                var cutOffDate = DateTime.Today.AddDays(-7);
                query = query.Where(r => r.Date >= cutOffDate);
            }
            var reservations = await query
                .OrderBy(r => r.Date)
                .ThenBy(r=> r.Time)
                .ToListAsync();

            return reservations.Select(ReservationMapper.ToIndexViewModel).ToList();

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

            return ReservationMapper.ToDetailsViewModel(reservation);

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
                Tables = tables
            };
        }

        public async Task AddReservationAsync(ReservationFormViewModel model)
        {
            if (!IsValidReservationDateTime(model.Date, model.Time))
            {
                throw new InvalidOperationException("Cannot create reservations for past dates or times.");
            }
            if (!await IsWithinOperatingHoursAsync(model.Time))
            {
                var settings = await _context.RestaurantSettings.FirstOrDefaultAsync();
                throw new InvalidOperationException(
               $"Invalid reservation time. Operating hours are {settings.OpeningHour:hh\\:mm} - {settings.ClosingHour:hh\\:mm}, last reservation accepted at {settings.ClosingHour - TimeSpan.FromHours(1):hh\\:mm}.");
            }
            if (!await TableHasEnoughSeatsAsync(model.TableId, model.NumberOfGuests))
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
                    Email = model.CustomerEmail,
                    Status = CustomerStatus.Regular
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (customer.Status == CustomerStatus.Blacklisted)
                {
                    throw new InvalidOperationException($"Cannot create reservation. Customer '{customer.FullName}' is blacklisted.");
                }
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

            if (!IsValidReservationDateTime(model.Date, model.Time))
            {
                throw new InvalidOperationException("Cannot create reservations for past dates or times.");
            }
            if (!await IsWithinOperatingHoursAsync(model.Time))
            {
                var settings = await _context.RestaurantSettings.FirstOrDefaultAsync();
                throw new InvalidOperationException(
               $"Invalid reservation time. Operating hours are {settings.OpeningHour:hh\\:mm} - {settings.ClosingHour:hh\\:mm}, last reservation accepted at {settings.ClosingHour - TimeSpan.FromHours(1):hh\\:mm}.");
            }
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
                  .FirstOrDefaultAsync(c => c.PhoneNumber == model.CustomerPhone &&
                                        c.FullName == model.CustomerName &&
                                        c.Email == model.CustomerEmail);

                if (existingCustomer == null)
                {
                    existingCustomer = await _context.Customers
                        .FirstOrDefaultAsync(c => c.PhoneNumber == model.CustomerPhone);
                }

                if (existingCustomer != null)
                {
                    if (existingCustomer.Status == CustomerStatus.Blacklisted)
                    {
                        throw new InvalidOperationException($"Cannot assign reservation to blacklisted customer '{existingCustomer.FullName}'.");
                    }

                    existingCustomer.FullName = model.CustomerName;
                    existingCustomer.Email = model.CustomerEmail;

                    reservation.CustomerId = existingCustomer.Id;
                }
                else
                {
                    var newCustomer = new Customer
                    {
                        FullName = model.CustomerName,
                        PhoneNumber = model.CustomerPhone,
                        Email = model.CustomerEmail,
                        Status = CustomerStatus.Regular
                    };
                    _context.Customers.Add(newCustomer);
                    await _context.SaveChangesAsync();
                    reservation.CustomerId = newCustomer.Id;
                }
            }
            else
            {
                if (reservation.Customer.Status == CustomerStatus.Blacklisted)
                {
                    throw new InvalidOperationException($"Cannot edit reservation. Customer '{reservation.Customer.FullName}' is blacklisted.");
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

            var reservations= await _context.Reservations
                .Where(r => r.Date.Date == today)
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .ToListAsync();

            return reservations.Select(ReservationMapper.ToIndexViewModel).ToList();
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

            var reservations = await _context.Reservations
           .Where(r =>
               r.TableId == tableId &&
               r.Date.Date == date.Date &&
               (!ignoreReservationId.HasValue || r.Id != ignoreReservationId.Value))
               .Select(r => r.Time)
               .ToListAsync();

            var hasConflict = reservations.Any(reservationTime =>
               reservationTime < end && reservationTime.Add(duration) > start
             );

            return !hasConflict;
        }

        public async Task<bool> IsWithinOperatingHoursAsync(TimeSpan time)
        {
            var settings = await _context.RestaurantSettings.FirstOrDefaultAsync();
            if (settings == null) return true;

            var minimumDiningTime = TimeSpan.FromHours(1);
            var latestAllowedTime = settings.ClosingHour - minimumDiningTime;

            return time >= settings.OpeningHour && time <= latestAllowedTime;
        }

        public bool IsValidReservationDateTime(DateTime date, TimeSpan time)
        {
            var reservationDateTime =  date.Date.Add(time);
            return reservationDateTime > DateTime.Now;
        }
    }
}
