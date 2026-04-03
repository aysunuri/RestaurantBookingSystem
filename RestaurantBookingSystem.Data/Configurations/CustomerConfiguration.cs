using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantBookingSystem.Data.Models;
using  RestaurantBookingSystem.Data.Models.Enums;

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
             new Customer { Id = 10,FullName = "Ava Scott", PhoneNumber = "0887675743", Email = "ava@example.com" },
             new Customer { Id = 11, FullName = "Cersei Lannister", PhoneNumber = "0890192819", Email = "theoneandonlycersei@example.com" },
             new Customer { Id = 12, FullName = "Royce Godwin", PhoneNumber = "0827282178", Email = "roycegood@example.com" },
             new Customer { Id = 13, FullName = "Eddy Moira", PhoneNumber = "0891910280", Email = "eddytedy@example.com" },
             new Customer { Id = 14, FullName = "Benjamin Ash", PhoneNumber = "08980116282", Email = "benash22@example.com" },
             new Customer { Id = 15, FullName = "Gavin Kyla", PhoneNumber = "08028291781", Email = "gavikylebro@example.com" },
             new Customer { Id = 16, FullName = "Theon Greyjoy", PhoneNumber = "08819272933", Email = "greyjoytheon@example.com" },
             new Customer { Id = 17, FullName = "Joffrey Baratheon", PhoneNumber = "0809939222", Email = "joffreydking@example.com" },
             new Customer { Id = 18, FullName = "Arya Stark", PhoneNumber = "0880958373", Email = "valarmorghulis@example.com", Status = CustomerStatus.VIP},
             new Customer { Id = 19, FullName = "Daenerys Targaryen", PhoneNumber = "0882927284", Email = "dmumodragons@example.com" },
             new Customer { Id = 20,FullName = "John Snow", PhoneNumber = "0887677467", Email = "aegontarg@example.com", Status = CustomerStatus.VIP }
        };
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.HasData(Customers);
        }
    }
}
