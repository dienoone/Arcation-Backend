using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class addSomeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "WorkingHours",
                table: "BandLocationLeaderPeriodEmployeePeriodAttendances",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WorkingHours",
                table: "Attendances",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "BandLocationLeaderPeriodEmployeePeriodAttendances");

            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "Attendances");
        }
    }
}
