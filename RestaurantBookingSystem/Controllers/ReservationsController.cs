using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.ViewModels.Reservation;


namespace RestaurantBookingSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReservationsController (ApplicationDbContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reservations =  await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .ToListAsync();

            var model = reservations.Select( r => new ReservationIndexViewModel
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
    }
}
