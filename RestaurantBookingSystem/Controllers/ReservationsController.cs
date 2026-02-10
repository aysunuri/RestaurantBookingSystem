using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Reservation;

namespace RestaurantBookingSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
          var model =await _reservationService.GetAllReservationsAsync();   
          return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _reservationService.GetReservationDetailsAsync(id);

            if (model== null)
             return NotFound();
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _reservationService.GetReservationFormModelAsync(0); // id = 0 is a new reservation
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Tables = await _reservationService.GetTablesDropDownAsync();
                return View(model);
            }

            try
            {
                await _reservationService.AddReservationAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Tables = await _reservationService.GetTablesDropDownAsync();
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _reservationService.GetReservationForEditAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ReservationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Tables = await _reservationService.GetTablesDropDownAsync();
                return View(model);
            }
            try
            {
                var success = await _reservationService.EditReservationAsync(model);

                if (!success)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Tables = await _reservationService.GetTablesDropDownAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _reservationService.GetReservationDetailsAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _reservationService.DeleteReservationAsync(id);

            if(!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
