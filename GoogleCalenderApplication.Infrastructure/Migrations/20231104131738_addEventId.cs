using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoogleCalenderApplication.Infrastructure.Migrations
{
    public partial class addEventId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventId",
                table: "CalenderEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                table: "CalenderEvents");
        }
    }
}
