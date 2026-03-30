using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBookingSystem.Data.Repository
{
    public abstract class BaseRepository : IDisposable
    {
        private bool isDisposed = false;
        private readonly ApplicationDbContext? dbContext;

        protected BaseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected ApplicationDbContext? DbContext => dbContext;

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext!.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    dbContext?.Dispose();
                }
            }
            isDisposed = true;
        }
    }
}
