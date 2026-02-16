using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels;
using System.Diagnostics;

namespace RestaurantBookingSystem.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IReservationService _reservationService;
        
        public HomeController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            var reservationsToday = await _reservationService.GetTodayReservationsAsync();

            return View(reservationsToday);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
