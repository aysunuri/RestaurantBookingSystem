using AutoMapper;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services.Contracts;
using RestaurantBookingSystem.ViewModels.Settings;

namespace RestaurantBookingSystem.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IMapper _mapper;

        public SettingsService(ISettingsRepository settingsRepository, IMapper mapper)
        {
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }

        public async Task<SettingsViewModel?> GetSettingsAsync()
        {
            var settings = await _settingsRepository.GetSettingsAsync();

            if (settings == null)
                return null;

            return _mapper.Map<SettingsViewModel>(settings);
        }

        public async Task<bool> UpdateSettingsAsync(SettingsViewModel model)
        {
            var settings = await _settingsRepository.GetSettingsAsync();

            if (settings == null)
                return false;

            _mapper.Map(model, settings);

            _settingsRepository.Update(settings);
            var result = await _settingsRepository.SaveChangesAsync();

            return true;
        }
    }
}