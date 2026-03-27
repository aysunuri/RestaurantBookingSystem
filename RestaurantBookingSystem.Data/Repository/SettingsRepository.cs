using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;

namespace RestaurantBookingSystem.Data.Repository
{
    public class SettingsRepository : BaseRepository, ISettingsRepository
    {
        public SettingsRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<RestaurantSettings?> GetSettingsAsync()
        {
            return await DbContext!.RestaurantSettings.FirstOrDefaultAsync();
        }

        public void Update(RestaurantSettings settings)
        {
            DbContext!.RestaurantSettings.Update(settings);
        }

        async Task<int> ISettingsRepository.SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
