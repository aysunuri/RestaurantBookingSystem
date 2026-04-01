using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantBookingSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBookingSystem.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasData(
                new Event { Id = 1, Name = "Pizza Day", Description = "All pizzas 20% off!", Date = new DateTime(2026, 4, 15), ImageUrl= "https://moneyinc.com/wp-content/uploads/2022/03/shutterstock_1614453529-750x490.jpg", IsActive = true },
                new Event { Id = 2, Name = "Taco Fiesta Night", Description = "Free shot with every taco set.", Date = new DateTime(2026, 5, 27), ImageUrl = "https://img.freepik.com/premium-photo/delicious-tacos_161767-1753.jpg", IsActive = true },
                new Event { Id = 3, Name = "Ladies Night", Description = "Special cocktails for ladies", Date = new DateTime(2026, 4, 25), ImageUrl = "https://www.mainandbroadmag.com/wp-content/uploads/2023/07/Nightingaleext2.jpg", IsActive = true },
                new Event { Id = 4, Name = "Sushi & Chill", Description = "Get 2-for-1 sushi rolls all night.", Date = new DateTime(2026,4, 20), ImageUrl = "https://i.pinimg.com/736x/ac/a8/f8/aca8f8463de190748b4505cdacce48eb.jpg", IsActive = true}
            );
        }
    }
}
