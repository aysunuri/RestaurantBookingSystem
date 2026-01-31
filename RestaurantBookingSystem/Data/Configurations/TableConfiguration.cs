using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantBookingSystem.Models;

namespace RestaurantBookingSystem.Data.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        private readonly IEnumerable<Table> Tables = new List<Table>
        {       
                new Table { Id = 1, TableNumber = 1, Seats = 4 },
                new Table { Id = 2, TableNumber = 2, Seats = 2 },
                new Table { Id = 3, TableNumber = 3, Seats = 6 }
        };
        public void Configure(EntityTypeBuilder<Table> entity)
        {
            entity.HasData(Tables);
        }
    }
}
