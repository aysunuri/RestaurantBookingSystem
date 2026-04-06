using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Reservation;

namespace RestaurantBookingSystem.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, bool showAll = false)
        {
            int pageSize = 10;

            var result = await _reservationService
                .GetPagedReservationsAsync(page, pageSize, showAll);
            ViewBag.ShowAll = showAll;

            return View(result);
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ReservationFormViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
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

        [HttpPost, ActionName("DeleteConfirmed")]
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
