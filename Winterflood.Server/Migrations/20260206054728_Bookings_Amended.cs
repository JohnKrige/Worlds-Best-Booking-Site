using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winterflood.Server.Migrations
{
    /// <inheritdoc />
    public partial class Bookings_Amended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "units",
                table: "Bookings",
                newName: "NumberOfItems");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "Bookings",
                newName: "BookingStartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingEndDate",
                table: "Bookings",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingEndDate",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "NumberOfItems",
                table: "Bookings",
                newName: "units");

            migrationBuilder.RenameColumn(
                name: "BookingStartDate",
                table: "Bookings",
                newName: "BookingDate");
        }
    }
}
