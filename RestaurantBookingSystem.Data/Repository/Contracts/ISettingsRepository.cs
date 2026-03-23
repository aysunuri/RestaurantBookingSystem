using RestaurantBookingSystem.Data.Models;

namespace RestaurantBookingSystem.Data.Repository.Contracts
{
    public interface ISettingsRepository
    {
        Task<RestaurantSettings?> GetSettingsAsync();
    }
}
