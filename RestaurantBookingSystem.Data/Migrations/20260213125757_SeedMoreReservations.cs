using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantBookingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "FullName", "PhoneNumber" },
                values: new object[,]
                {
                    { 3, "michael@example.com", "Michael Brown", "0896343527" },
                    { 4, "sarah@example.com", "Sarah Davis", "0876524259" },
                    { 5, "daniel@example.com", "Daniel Green", "086789212" },
                    { 6, "emma@example.com", "Emma Wilson", "0897645432" },
                    { 7, "oliver@example.com", "Oliver King", "0896565743" },
                    { 8, "sophia@example.com", "Sophia Turner", "0885431326" },
                    { 9, "james@example.com", "James Hall", "0886574393" },
                    { 10, "ava@example.com", "Ava Scott", "0887675743" }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CustomerId", "Date", "Notes", "NumberOfGuests", "TableId", "Time" },
                values: new object[,]
                {
                    { 3, 3, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anniversary", 3, 3, new TimeSpan(0, 18, 0, 0, 0) },
                    { 4, 4, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Date night", 2, 4, new TimeSpan(0, 20, 0, 0, 0) },
                    { 5, 5, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business lunch", 5, 3, new TimeSpan(0, 13, 0, 0, 0) },
                    { 6, 6, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Early dinner", 2, 1, new TimeSpan(0, 17, 30, 0, 0) },
                    { 7, 7, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Friends gathering", 6, 5, new TimeSpan(0, 21, 0, 0, 0) },
                    { 8, 8, new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Valentine's Day", 2, 4, new TimeSpan(0, 19, 30, 0, 0) },
                    { 9, 9, new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Casual lunch", 3, 2, new TimeSpan(0, 12, 0, 0, 0) },
                    { 10, 10, new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Double date", 4, 1, new TimeSpan(0, 18, 45, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
