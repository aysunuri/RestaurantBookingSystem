using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantBookingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreReservations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "FullName", "Notes", "PhoneNumber", "Status" },
                values: new object[,]
                {
                    { 11, "theoneandonlycersei@example.com", "Cersei Lannister", null, "0890192819", 0 },
                    { 12, "roycegood@example.com", "Royce Godwin", null, "0827282178", 0 },
                    { 13, "eddytedy@example.com", "Eddy Moira", null, "0891910280", 0 },
                    { 14, "benash22@example.com", "Benjamin Ash", null, "08980116282", 0 },
                    { 15, "gavikylebro@example.com", "Gavin Kyla", null, "08028291781", 0 },
                    { 16, "greyjoytheon@example.com", "Theon Greyjoy", null, "08819272933", 0 },
                    { 17, "joffreydking@example.com", "Joffrey Baratheon", null, "0809939222", 0 },
                    { 18, "valarmorghulis@example.com", "Arya Stark", null, "0880958373", 1 },
                    { 19, "dmumodragons@example.com", "Daenerys Targaryen", null, "0882927284", 0 },
                    { 20, "aegontarg@example.com", "John Snow", null, "0887677467", 1 }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CustomerId", "Date", "Notes", "NumberOfGuests", "TableId", "Time" },
                values: new object[,]
                {
                    { 11, 20, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Birthday celebration", 2, 3, new TimeSpan(0, 19, 0, 0, 0) },
                    { 12, 19, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family dinner", 4, 4, new TimeSpan(0, 12, 30, 0, 0) },
                    { 13, 18, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anniversary dinner", 3, 11, new TimeSpan(0, 18, 0, 0, 0) },
                    { 14, 17, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Date night", 2, 1, new TimeSpan(0, 20, 0, 0, 0) },
                    { 15, 16, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business dinner", 5, 14, new TimeSpan(0, 13, 0, 0, 0) },
                    { 16, 15, new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Early dinner", 2, 10, new TimeSpan(0, 17, 30, 0, 0) },
                    { 17, 14, new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Friends gathering", 6, 6, new TimeSpan(0, 21, 0, 0, 0) },
                    { 18, 13, new DateTime(2026, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brunch with the girls", 2, 1, new TimeSpan(0, 19, 30, 0, 0) },
                    { 19, 12, new DateTime(2026, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Casual lunch", 3, 11, new TimeSpan(0, 12, 0, 0, 0) },
                    { 20, 11, new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Double date", 4, 12, new TimeSpan(0, 18, 45, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
