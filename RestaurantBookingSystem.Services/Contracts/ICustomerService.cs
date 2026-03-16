using RestaurantBookingSystem.ViewModels.Customer;

namespace RestaurantBookingSystem.Services.Contracts
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerIndexViewModel>> GetAllCustomersAsync();

        Task<IEnumerable<CustomerIndexViewModel>> SearchCustomersAsync(string searchTerm);

        Task<CustomerDetailsViewModel?> GetCustomerDetailsAsync(int id);

        Task<CustomerEditViewModel?> GetCustomerForEditAsync(int id);

        Task<bool> UpdateCustomerStatusAsync(CustomerEditViewModel model);
    }
}
