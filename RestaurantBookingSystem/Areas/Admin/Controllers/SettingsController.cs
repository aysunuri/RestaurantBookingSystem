using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Settings;

namespace RestaurantBookingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = await _settingsService.GetSettingsAsync();

            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var settings = await _settingsService.GetSettingsAsync();

            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,SettingsViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ClosingHour != TimeSpan.Zero && model.ClosingHour <= model.OpeningHour)
            {
                ModelState.AddModelError("ClosingHour", "Closing hour must be after opening hour.");
                return View(model);
            }

            var success = await _settingsService.UpdateSettingsAsync(model);

            if (!success)
            {
                return NotFound();
            }

            TempData["Success"] = "Restaurant settings updated successfully.";
            return RedirectToAction(nameof(Index));

        }
    }
}