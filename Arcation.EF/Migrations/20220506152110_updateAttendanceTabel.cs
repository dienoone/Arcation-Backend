using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class updateAttendanceTabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AttendanceState",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ended",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceState",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ended",
                table: "Attendances");
        }
    }
}
