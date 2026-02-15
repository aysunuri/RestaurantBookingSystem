using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels;
using System.Diagnostics;

namespace RestaurantBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReservationService _reservationService;
        
        public HomeController(ILogger<HomeController> logger, IReservationService reservationService)
        {
            _logger = logger;
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
