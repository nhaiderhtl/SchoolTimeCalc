using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolTimeCalc.Migrations
{
    public partial class AddUntisUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "WebUntisData",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "WebUntisData");
        }
    }
}
