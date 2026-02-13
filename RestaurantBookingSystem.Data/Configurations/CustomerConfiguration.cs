using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantBookingSystem.Data.Models;

namespace RestaurantBookingSystem.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        private static readonly IEnumerable<Customer> Customers = new List<Customer>
        {
             new Customer { Id = 1, FullName = "John Doe", PhoneNumber = "0888123456", Email = "john@example.com" },
             new Customer { Id = 2, FullName = "Maria Ivanova", PhoneNumber = "0899123456", Email = "maria@example.com" },
             new Customer { Id = 3, FullName = "Michael Brown", PhoneNumber = "0896343527", Email = "michael@example.com" },
             new Customer { Id = 4, FullName = "Sarah Davis", PhoneNumber = "0876524259", Email = "sarah@example.com" },
             new Customer { Id = 5, FullName = "Daniel Green", PhoneNumber = "086789212", Email = "daniel@example.com" },
             new Customer { Id = 6, FullName = "Emma Wilson", PhoneNumber = "0897645432", Email = "emma@example.com" },
             new Customer { Id = 7, FullName = "Oliver King", PhoneNumber = "0896565743", Email = "oliver@example.com" },
             new Customer { Id = 8, FullName = "Sophia Turner", PhoneNumber = "0885431326", Email = "sophia@example.com" },
             new Customer { Id = 9, FullName = "James Hall", PhoneNumber = "0886574393", Email = "james@example.com" },
             new Customer { Id = 10,FullName = "Ava Scott", PhoneNumber = "0887675743", Email = "ava@example.com" }
        };
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.HasData(Customers);
        }
    }
}
