using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class somePiceOfCake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndingDate",
                table: "BandLocationLeaderPeriods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PayiedState",
                table: "BandLocationLeaderPeriods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartingDate",
                table: "BandLocationLeaderPeriods",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "BandLocationLeaderPeriods",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<double>(
                name: "BorrowValue",
                table: "Attendances",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingDate",
                table: "BandLocationLeaderPeriods");

            migrationBuilder.DropColumn(
                name: "PayiedState",
                table: "BandLocationLeaderPeriods");

            migrationBuilder.DropColumn(
                name: "StartingDate",
                table: "BandLocationLeaderPeriods");

            migrationBuilder.DropColumn(
                name: "State",
                table: "BandLocationLeaderPeriods");

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

            migrationBuilder.DropColumn(
                name: "BorrowValue",
                table: "Attendances");
        }
    }
}
