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

            return RedirectToAction(nameof(Index));
        }
    }
}
