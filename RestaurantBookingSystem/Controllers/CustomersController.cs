using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Customer;

namespace RestaurantBookingSystem.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            IEnumerable<CustomerIndexViewModel> customers;

            if (!string.IsNullOrWhiteSpace(search))
            {
                customers = await _customerService.SearchCustomersAsync(search);
                ViewData["CurrentSearch"] = search;
            }
            else
            {
                customers = await _customerService.GetAllCustomersAsync();
            }

            return View(customers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetCustomerDetailsAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _customerService.GetCustomerForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _customerService.UpdateCustomerStatusAsync(model);

            if (!success)
            {
                return NotFound();
            }

            TempData["Success"] = "Customer status updated successfully.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
    }
}