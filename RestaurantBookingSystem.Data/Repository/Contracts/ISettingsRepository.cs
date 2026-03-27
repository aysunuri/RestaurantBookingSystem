using RestaurantBookingSystem.Data.Models;

namespace RestaurantBookingSystem.Data.Repository.Contracts
{
    public interface ISettingsRepository
    {
        Task<RestaurantSettings?> GetSettingsAsync();
        void Update(RestaurantSettings settings);

        Task<int> SaveChangesAsync();
    }
}
