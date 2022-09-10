using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class RemoveExtractRowMeters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtractRowMeters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtractRowMeters",
                columns: table => new
                {
                    ExtractRowMeterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtractRowId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Needed = table.Column<bool>(type: "bit", nullable: false),
                    Now = table.Column<double>(type: "float", nullable: false),
                    Previous = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractRowMeters", x => x.ExtractRowMeterId);
                    table.ForeignKey(
                        name: "FK_ExtractRowMeters_ExtractRows_ExtractRowId",
                        column: x => x.ExtractRowId,
                        principalTable: "ExtractRows",
                        principalColumn: "ExtractRowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtractRowMeters_ExtractRowId",
                table: "ExtractRowMeters",
                column: "ExtractRowId");
        }
    }
}
