using RestaurantBookingSystem.ViewModels.Customer;
using RestaurantBookingSystem.ViewModels.Shared;

namespace RestaurantBookingSystem.Services.Contracts
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerIndexViewModel>> GetAllCustomersAsync();

        Task<IEnumerable<CustomerIndexViewModel>> SearchCustomersAsync(string searchTerm);
        Task<PagedResult<CustomerIndexViewModel>> GetPagedCustomersAsync(int page, int pageSize, string? searchTerm = null);

        Task<CustomerDetailsViewModel?> GetCustomerDetailsAsync(int id);

        Task<CustomerEditViewModel?> GetCustomerForEditAsync(int id);

        Task<bool> UpdateCustomerStatusAsync(CustomerEditViewModel model);
    }
}
