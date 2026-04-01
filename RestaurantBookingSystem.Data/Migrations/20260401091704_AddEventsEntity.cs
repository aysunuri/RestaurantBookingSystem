using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantBookingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEventsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Date", "Description", "ImageUrl", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "All pizzas 20% off!", "https://moneyinc.com/wp-content/uploads/2022/03/shutterstock_1614453529-750x490.jpg", true, "Pizza Day" },
                    { 2, new DateTime(2026, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Free shot with every taco set.", "https://img.freepik.com/premium-photo/delicious-tacos_161767-1753.jpg", true, "Taco Fiesta Night" },
                    { 3, new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Special cocktails for ladies", "https://www.mainandbroadmag.com/wp-content/uploads/2023/07/Nightingaleext2.jpg", true, "Ladies Night" },
                    { 4, new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Get 2-for-1 sushi rolls all night.", "https://i.pinimg.com/736x/ac/a8/f8/aca8f8463de190748b4505cdacce48eb.jpg", true, "Sushi & Chill" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
