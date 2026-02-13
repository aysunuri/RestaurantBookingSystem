using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantBookingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantSettingsTableWithOperatingHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpeningHour = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingHour = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "Id", "ClosingHour", "OpeningHour" },
                values: new object[] { 1, new TimeSpan(0, 23, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantSettings");
        }
    }
}
