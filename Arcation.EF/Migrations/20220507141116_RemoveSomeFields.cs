using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class RemoveSomeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalBorrow",
                table: "BandLocationLeaderPeriods");

            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "BandLocationLeaderPeriods");

            migrationBuilder.DropColumn(
                name: "TotalRemainder",
                table: "BandLocationLeaderPeriods");

            migrationBuilder.DropColumn(
                name: "TotalSalary",
                table: "BandLocationLeaderPeriods");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalBorrow",
                table: "BandLocationLeaderPeriods",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalDays",
                table: "BandLocationLeaderPeriods",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalRemainder",
                table: "BandLocationLeaderPeriods",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalSalary",
                table: "BandLocationLeaderPeriods",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
