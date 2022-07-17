using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class addSomeFieldsToLeaderPeriodEmployeeContainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndingDate",
                table: "BandLocationLeaderPeriodEmployees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartingDate",
                table: "BandLocationLeaderPeriodEmployees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingDate",
                table: "BandLocationLeaderPeriodEmployees");

            migrationBuilder.DropColumn(
                name: "StartingDate",
                table: "BandLocationLeaderPeriodEmployees");
        }
    }
}
