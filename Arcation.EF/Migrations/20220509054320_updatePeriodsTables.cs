using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class updatePeriodsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "BandLocationLeaderPeriodEmployeePeriods");

            migrationBuilder.RenameColumn(
                name: "TotalSalary",
                table: "BandLocationLeaderPeriodEmployeePeriods",
                newName: "PayiedValue");

            migrationBuilder.RenameColumn(
                name: "TotalRemainder",
                table: "BandLocationLeaderPeriodEmployeePeriods",
                newName: "EmployeeSalary");

            migrationBuilder.AddColumn<double>(
                name: "LeaderSalary",
                table: "BandLocationLeaderPeriods",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaderSalary",
                table: "BandLocationLeaderPeriods");

            migrationBuilder.RenameColumn(
                name: "PayiedValue",
                table: "BandLocationLeaderPeriodEmployeePeriods",
                newName: "TotalSalary");

            migrationBuilder.RenameColumn(
                name: "EmployeeSalary",
                table: "BandLocationLeaderPeriodEmployeePeriods",
                newName: "TotalRemainder");

            migrationBuilder.AddColumn<double>(
                name: "TotalDays",
                table: "BandLocationLeaderPeriodEmployeePeriods",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
