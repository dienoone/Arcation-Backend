using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class SomeChangesIdontKnowWhatIsThis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Extract_BandLocations_BandLocationId",
                table: "Extract");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtractRow_Extract_ExtractId",
                table: "ExtractRow");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtractRowMeter_ExtractRow_ExtractRowId",
                table: "ExtractRowMeter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtractRowMeter",
                table: "ExtractRowMeter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtractRow",
                table: "ExtractRow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Extract",
                table: "Extract");

            migrationBuilder.RenameTable(
                name: "ExtractRowMeter",
                newName: "ExtractRowMeters");

            migrationBuilder.RenameTable(
                name: "ExtractRow",
                newName: "ExtractRows");

            migrationBuilder.RenameTable(
                name: "Extract",
                newName: "Extracts");

            migrationBuilder.RenameIndex(
                name: "IX_ExtractRowMeter_ExtractRowId",
                table: "ExtractRowMeters",
                newName: "IX_ExtractRowMeters_ExtractRowId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtractRow_ExtractId",
                table: "ExtractRows",
                newName: "IX_ExtractRows_ExtractId");

            migrationBuilder.RenameIndex(
                name: "IX_Extract_BandLocationId",
                table: "Extracts",
                newName: "IX_Extracts_BandLocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtractRowMeters",
                table: "ExtractRowMeters",
                column: "ExtractRowMeterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtractRows",
                table: "ExtractRows",
                column: "ExtractRowId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Extracts",
                table: "Extracts",
                column: "ExtractId");

            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    ToolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.ToolId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ExtractRowMeters_ExtractRows_ExtractRowId",
                table: "ExtractRowMeters",
                column: "ExtractRowId",
                principalTable: "ExtractRows",
                principalColumn: "ExtractRowId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtractRows_Extracts_ExtractId",
                table: "ExtractRows",
                column: "ExtractId",
                principalTable: "Extracts",
                principalColumn: "ExtractId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Extracts_BandLocations_BandLocationId",
                table: "Extracts",
                column: "BandLocationId",
                principalTable: "BandLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtractRowMeters_ExtractRows_ExtractRowId",
                table: "ExtractRowMeters");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtractRows_Extracts_ExtractId",
                table: "ExtractRows");

            migrationBuilder.DropForeignKey(
                name: "FK_Extracts_BandLocations_BandLocationId",
                table: "Extracts");

            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Extracts",
                table: "Extracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtractRows",
                table: "ExtractRows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtractRowMeters",
                table: "ExtractRowMeters");

            migrationBuilder.RenameTable(
                name: "Extracts",
                newName: "Extract");

            migrationBuilder.RenameTable(
                name: "ExtractRows",
                newName: "ExtractRow");

            migrationBuilder.RenameTable(
                name: "ExtractRowMeters",
                newName: "ExtractRowMeter");

            migrationBuilder.RenameIndex(
                name: "IX_Extracts_BandLocationId",
                table: "Extract",
                newName: "IX_Extract_BandLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtractRows_ExtractId",
                table: "ExtractRow",
                newName: "IX_ExtractRow_ExtractId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtractRowMeters_ExtractRowId",
                table: "ExtractRowMeter",
                newName: "IX_ExtractRowMeter_ExtractRowId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Extract",
                table: "Extract",
                column: "ExtractId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtractRow",
                table: "ExtractRow",
                column: "ExtractRowId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtractRowMeter",
                table: "ExtractRowMeter",
                column: "ExtractRowMeterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Extract_BandLocations_BandLocationId",
                table: "Extract",
                column: "BandLocationId",
                principalTable: "BandLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtractRow_Extract_ExtractId",
                table: "ExtractRow",
                column: "ExtractId",
                principalTable: "Extract",
                principalColumn: "ExtractId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtractRowMeter_ExtractRow_ExtractRowId",
                table: "ExtractRowMeter",
                column: "ExtractRowId",
                principalTable: "ExtractRow",
                principalColumn: "ExtractRowId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
