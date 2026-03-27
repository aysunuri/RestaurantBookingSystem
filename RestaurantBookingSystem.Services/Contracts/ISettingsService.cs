using RestaurantBookingSystem.ViewModels.Settings;

namespace RestaurantBookingSystem.Services.Contracts
{
    public interface ISettingsService
    {
        Task<SettingsViewModel?> GetSettingsAsync();
        Task<bool> UpdateSettingsAsync(SettingsViewModel model);
    }
}