using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Models.Enums;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Mappers;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels;
using RestaurantBookingSystem.ViewModels.Reservation;

namespace RestaurantBookingSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITableRepository _tableRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            ICustomerRepository customerRepository,
            ITableRepository tableRepository)
        {
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;
            _tableRepository = tableRepository;
        }
        public async Task<IEnumerable<ReservationIndexViewModel>> GetAllReservationsAsync(bool includeOld = false)
        {
            IEnumerable<Reservation> reservations;

            if (includeOld)
            {
                reservations = await _reservationRepository.GetAllWithDetailsAsync();
            }
            else
            {
                reservations = await _reservationRepository.GetRecentReservationsAsync(7);
            }

            return reservations.Select(ReservationMapper.ToIndexViewModel).ToList();

        }
        public async Task<ReservationDetailsViewModel?> GetReservationDetailsAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdWithDetailsAsync(id);

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

            var reservation = await _reservationRepository.GetByIdWithCustomerAsync(id);

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
                throw new InvalidOperationException(
                    "Invalid reservation time. Operating hours are 10:00 - 23:00, last reservation accepted at 22:00.");
            }
            if (!await TableHasEnoughSeatsAsync(model.TableId, model.NumberOfGuests))
            {
                throw new InvalidOperationException("The selected table doesn't have enough seats for the number of guests.");
            }
            if (!await TableIsAvailableAsync(model.TableId, model.Date, model.Time))
            {
                throw new InvalidOperationException("The selected table is already booked for this time slot.");
            }

            var customer = await _customerRepository.GetByPhoneNumberAsync(model.CustomerPhone);

            if (customer == null)
            {
                customer = new Customer
                {
                    FullName = model.CustomerName,
                    PhoneNumber = model.CustomerPhone,
                    Email = model.CustomerEmail,
                    Status = CustomerStatus.Regular
                };

                await _customerRepository.AddAsync(customer);
                await _reservationRepository.SaveChangesAsync();
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

            await _reservationRepository.AddAsync(reservation); 
            await _reservationRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DropDownItemViewModel>> GetTablesDropDownAsync()
        {
            var tables = await _tableRepository.GetAllAsync();
            return tables.Select(t => new DropDownItemViewModel
             {
                 Value = t.Id,
                 Text = $"Table {t.TableNumber} - {t.Seats} seats"
             })
            .ToList();
        }
        public async Task<ReservationFormViewModel?> GetReservationForEditAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdWithDetailsAsync(id);

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

            var reservation = await _reservationRepository.GetByIdWithCustomerAsync(model.Id!.Value);

            if (reservation == null)
                return false;

            if (!IsValidReservationDateTime(model.Date, model.Time))
            {
                throw new InvalidOperationException("Cannot create reservations for past dates or times.");
            }
            if (!await IsWithinOperatingHoursAsync(model.Time))
            {
                throw new InvalidOperationException(
                   "Invalid reservation time. Operating hours are 10:00 - 23:00, last reservation accepted at 22:00.");
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
                var existingCustomer = await _customerRepository.GetByExactMatchAsync(
                    model.CustomerName,
                    model.CustomerPhone,
                    model.CustomerEmail);

                if (existingCustomer == null)
                {
                    existingCustomer = await _customerRepository
                        .GetByPhoneNumberAsync(model.CustomerPhone);
                }

                if (existingCustomer != null)
                {
                    if (existingCustomer.Status == CustomerStatus.Blacklisted)
                    {
                        throw new InvalidOperationException($"Cannot assign reservation to blacklisted customer '{existingCustomer.FullName}'.");
                    }

                    existingCustomer.FullName = model.CustomerName;
                    existingCustomer.Email = model.CustomerEmail;

                    _customerRepository.Update(existingCustomer);  
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

                    await _customerRepository.AddAsync(newCustomer);
                    await _reservationRepository.SaveChangesAsync();
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

            _reservationRepository.Update(reservation);  
            await _reservationRepository.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteReservationAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (reservation == null)
                return false;

            _reservationRepository.Delete(reservation);
            await _reservationRepository.SaveChangesAsync();

            return true;  
        }


        public async Task<IEnumerable<ReservationIndexViewModel>> GetTodayReservationsAsync()
        {
            var today = DateTime.Today;

            var reservations = await _reservationRepository.GetTodayReservationsAsync();

            return reservations.Select(ReservationMapper.ToIndexViewModel).ToList();
        }

        public async Task<bool> TableHasEnoughSeatsAsync(int tableId, int guests)
        {
            return await _tableRepository.TableHasEnoughSeatsAsync(tableId, guests);
        }

        public async Task<bool> TableIsAvailableAsync(int tableId, DateTime date, TimeSpan time, int? ignoreReservationId = null)
        {
            return await _reservationRepository.IsTableAvailableAsync(tableId, date, time, ignoreReservationId);
        }

        public async Task<bool> IsWithinOperatingHoursAsync(TimeSpan time)
        {
            // Hardcoded for now - will use SettingsRepository later
            var openingHour = new TimeSpan(10, 0, 0);
            var closingHour = new TimeSpan(23, 0, 0);
            var minimumDiningTime = TimeSpan.FromHours(1);
            var latestAllowedTime = closingHour - minimumDiningTime;

            return time >= openingHour && time <= latestAllowedTime;
        }

        public bool IsValidReservationDateTime(DateTime date, TimeSpan time)
        {
            var reservationDateTime =  date.Date.Add(time);
            return reservationDateTime > DateTime.Now;
        }
    }
}
