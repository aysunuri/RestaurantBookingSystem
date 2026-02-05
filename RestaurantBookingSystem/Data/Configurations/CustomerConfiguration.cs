using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantBookingSystem.Models;

namespace RestaurantBookingSystem.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        private static readonly IEnumerable<Customer> Customers = new List<Customer>
        {
             new Customer { Id = 1, FullName = "John Doe", PhoneNumber = "0888123456", Email = "john@example.com" },
             new Customer { Id = 2, FullName = "Maria Ivanova", PhoneNumber = "0899123456", Email = "maria@example.com" }
        };
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.HasData(Customers);
        }
    }
}
