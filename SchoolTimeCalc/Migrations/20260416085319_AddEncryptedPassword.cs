using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolTimeCalc.Migrations
{
    /// <inheritdoc />
    public partial class AddEncryptedPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EncryptedPassword",
                table: "WebUntisData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastHolidaySync",
                table: "WebUntisData",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Server",
                table: "WebUntisData",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptedPassword",
                table: "WebUntisData");

            migrationBuilder.DropColumn(
                name: "LastHolidaySync",
                table: "WebUntisData");

            migrationBuilder.DropColumn(
                name: "Server",
                table: "WebUntisData");
        }
    }
}
