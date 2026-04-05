using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels;
using System.Diagnostics;

namespace RestaurantBookingSystem.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IEventService _eventService;
        private readonly ISettingsService _settingsService;
        
        public HomeController(
            IReservationService reservationService,
            IEventService eventService,
            ISettingsService settingsService)
        {
            _reservationService = reservationService;
            _eventService = eventService;
            _settingsService = settingsService;
        }

        public async Task<IActionResult> Index()
        {
            var reservationsToday = await _reservationService.GetTodayReservationsAsync();
            var events = await _eventService.GetActiveEventsAsync();
            var settings = await _settingsService.GetSettingsAsync();

            ViewBag.Events = events;
            ViewBag.OpeningHour = settings?.OpeningHour.ToString(@"hh\:mm") ?? "10:00";
            ViewBag.ClosingHour = settings?.ClosingHour.ToString(@"hh\:mm") ?? "22:00";

            return View(reservationsToday);
        }

        [Route("Home/Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == StatusCodes.Status400BadRequest)
            {
                return View("BadRequest");
            }

            if (statusCode == StatusCodes.Status404NotFound)
            {
                return View("NotFound");
            }

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                return View("ServerError");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
