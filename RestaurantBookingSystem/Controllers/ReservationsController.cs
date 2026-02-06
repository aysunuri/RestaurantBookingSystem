using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Models;
using RestaurantBookingSystem.ViewModels.Reservation;


namespace RestaurantBookingSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReservationsController(ApplicationDbContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .ToListAsync();

            var model = reservations.Select(r => new ReservationIndexViewModel
            {
                Id = r.Id,
                Date = r.Date.ToShortDateString(),
                Time = r.Time.ToString(@"hh\:mm"),
                NumberOfGuests = r.NumberOfGuests,
                CustomerName = r.Customer.FullName,
                TableNumber = r.Table.TableNumber
            }).ToList();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            var model = new ReservationDetailsViewModel
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

            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            var model = new ReservationFormViewModel()
            {
                Date = DateTime.Today,
                Tables = await _context.Tables
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"Table {t.TableNumber} - {t.Seats} seats"
                })
                .ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Tables = await _context.Tables
              .Select(t => new SelectListItem
              {
                  Value = t.Id.ToString(),
                  Text = $"Table {t.TableNumber} - {t.Seats} seats"
              })
              .ToListAsync();

                return View(model);
            }

            //Cheking if the customer already exist in the Db
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == model.CustomerPhone);

            //If not, create a new customer
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

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            var model = new ReservationFormViewModel
            {
                Id = reservation.Id,
                Date = reservation.Date,
                Time = reservation.Time,
                NumberOfGuests = reservation.NumberOfGuests,
                Notes = reservation.Notes,
                CustomerName = reservation.Customer.FullName,
                CustomerPhone = reservation.Customer.PhoneNumber,
                CustomerEmail = reservation.Customer.Email,
                TableId = reservation.TableId,
                Tables = await _context.Tables
                 .Select(t => new SelectListItem
                 {
                     Value = t.Id.ToString(),
                     Text = $"Table {t.TableNumber} - {t.Seats} seats"
                 })
                 .ToListAsync()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReservationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Tables = await _context.Tables
              .Select(t => new SelectListItem
              {
                  Value = t.Id.ToString(),
                  Text = $"Table {t.TableNumber} - {t.Seats} seats"
              })
              .ToListAsync();

                return View(model);
            }

            var reservation = await _context.Reservations
               .Include(r => r.Customer)
               .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            reservation.Customer.FullName = model.CustomerName;
            reservation.Customer.PhoneNumber = model.CustomerPhone;
            reservation.Customer.Email = model.CustomerEmail;
            reservation.Date = model.Date;
            reservation.Time = model.Time;
            reservation.NumberOfGuests = model.NumberOfGuests;
            reservation.Notes = model.Notes;
            reservation.TableId = model.TableId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
