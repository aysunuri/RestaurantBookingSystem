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

    }
}
