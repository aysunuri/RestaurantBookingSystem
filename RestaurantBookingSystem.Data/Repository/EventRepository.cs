using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository;
using RestaurantBookingSystem.Data.Repository.Contracts;

namespace RestaurantBookingSystem.Data.Repositories
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        public EventRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await DbContext!.Events
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetActiveEventsAsync()
        {
            return await DbContext!.Events
                .Where(e => e.IsActive && e.Date >= DateTime.Today)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await DbContext!.Events.FindAsync(id);
        }

        public async Task AddAsync(Event eventEntity)
        {
            await DbContext!.Events.AddAsync(eventEntity);
        }

        public void Update(Event eventEntity)
        {
            DbContext!.Events.Update(eventEntity);
        }

        public void Delete(Event eventEntity)
        {
            DbContext!.Events.Remove(eventEntity);
        }

        public new async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}