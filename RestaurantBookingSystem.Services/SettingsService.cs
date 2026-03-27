using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Settings;

namespace RestaurantBookingSystem.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<SettingsViewModel?> GetSettingsAsync()
        {
            var settings = await _settingsRepository.GetSettingsAsync();

            if (settings == null)
                return null;

            return new SettingsViewModel
            {
                Id = settings.Id,
                RestaurantName = settings.RestaurantName,
                OpeningHour = settings.OpeningHour,
                ClosingHour = settings.ClosingHour
            };
        }

        public async Task<bool> UpdateSettingsAsync(SettingsViewModel model)
        {
            var settings = await _settingsRepository.GetSettingsAsync();

            if (settings == null)
                return false;

            settings.RestaurantName = model.RestaurantName;
            settings.OpeningHour = model.OpeningHour;
            settings.ClosingHour = model.ClosingHour;

            _settingsRepository.Update(settings);
            await _settingsRepository.SaveChangesAsync();

            return true;
        }
    }
}