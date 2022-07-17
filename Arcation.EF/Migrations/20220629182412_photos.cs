using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class photos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Extract",
                columns: table => new
                {
                    ExtractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExtractName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BandLocationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extract", x => x.ExtractId);
                    table.ForeignKey(
                        name: "FK_Extract_BandLocations_BandLocationId",
                        column: x => x.BandLocationId,
                        principalTable: "BandLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtractRow",
                columns: table => new
                {
                    ExtractRowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstatementUnite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstatementUnitePrice = table.Column<double>(type: "float", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtractId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractRow", x => x.ExtractRowId);
                    table.ForeignKey(
                        name: "FK_ExtractRow_Extract_ExtractId",
                        column: x => x.ExtractId,
                        principalTable: "Extract",
                        principalColumn: "ExtractId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtractRowMeter",
                columns: table => new
                {
                    ExtractRowMeterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Previous = table.Column<double>(type: "float", nullable: false),
                    Now = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Needed = table.Column<bool>(type: "bit", nullable: false),
                    ExtractRowId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractRowMeter", x => x.ExtractRowMeterId);
                    table.ForeignKey(
                        name: "FK_ExtractRowMeter_ExtractRow_ExtractRowId",
                        column: x => x.ExtractRowId,
                        principalTable: "ExtractRow",
                        principalColumn: "ExtractRowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Extract_BandLocationId",
                table: "Extract",
                column: "BandLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractRow_ExtractId",
                table: "ExtractRow",
                column: "ExtractId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractRowMeter_ExtractRowId",
                table: "ExtractRowMeter",
                column: "ExtractRowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtractRowMeter");

            migrationBuilder.DropTable(
                name: "ExtractRow");

            migrationBuilder.DropTable(
                name: "Extract");
        }
    }
}
