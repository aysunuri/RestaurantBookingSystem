using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantBookingSystem.Data.Models;

namespace RestaurantBookingSystem.Data.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        private static readonly IEnumerable<Table> Tables = new List<Table>
        {       
                new Table { Id = 1, TableNumber = 1, Seats = 2 },
                new Table { Id = 2, TableNumber = 2, Seats = 4 },
                new Table { Id = 3, TableNumber = 3, Seats = 2 },
                new Table { Id = 4, TableNumber = 4, Seats = 4 },
                new Table { Id = 5, TableNumber = 5, Seats = 6 },
                new Table { Id = 6, TableNumber = 6, Seats = 6 },
                new Table { Id = 7, TableNumber = 7, Seats = 10 },
                new Table { Id = 8, TableNumber = 8, Seats = 10 },
                new Table { Id = 9, TableNumber = 9, Seats = 2 },
                new Table { Id = 10, TableNumber = 10, Seats = 2 },
                new Table { Id = 11, TableNumber = 11, Seats = 4 },
                new Table { Id = 12, TableNumber = 12, Seats = 4 },
                new Table { Id = 13, TableNumber = 13, Seats = 6 },
                new Table { Id = 14, TableNumber = 14, Seats = 6 },
                new Table { Id = 15, TableNumber = 15, Seats = 10 },
                new Table { Id = 16, TableNumber = 16, Seats = 10 },
                new Table { Id = 17, TableNumber = 17, Seats = 2 },
                new Table { Id = 18, TableNumber = 18, Seats = 8 },
                new Table { Id = 19, TableNumber = 19, Seats = 8 },
                new Table { Id = 20, TableNumber = 20, Seats = 20 },
        };
        public void Configure(EntityTypeBuilder<Table> entity)
        {
            entity.HasData(Tables);
        }
    }
}
