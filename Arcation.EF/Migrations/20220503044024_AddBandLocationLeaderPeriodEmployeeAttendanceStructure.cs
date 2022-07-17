using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcation.EF.Migrations
{
    public partial class AddBandLocationLeaderPeriodEmployeeAttendanceStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandCompanies");

            migrationBuilder.AddColumn<string>(
                name: "BusinessId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BillPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillPrice = table.Column<double>(type: "float", nullable: false),
                    BillNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BandLocationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bills_BandLocations_BandLocationId",
                        column: x => x.BandLocationId,
                        principalTable: "BandLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BLWesteds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BandLocationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLWesteds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BLWesteds_BandLocations_BandLocationId",
                        column: x => x.BandLocationId,
                        principalTable: "BandLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    IncomeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    BandLocationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.IncomeId);
                    table.ForeignKey(
                        name: "FK_Incomes_BandLocations_BandLocationId",
                        column: x => x.BandLocationId,
                        principalTable: "BandLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leaders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passwrod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    BandLocationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Periods_BandLocations_BandLocationId",
                        column: x => x.BandLocationId,
                        principalTable: "BandLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "EmployeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BandLocationLeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BandLocationId = table.Column<int>(type: "int", nullable: false),
                    BandId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandLocationLeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaders_BandLocations_BandLocationId",
                        column: x => x.BandLocationId,
                        principalTable: "BandLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaders_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaders_Leaders_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Leaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BandLocationLeaderPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodId = table.Column<int>(type: "int", nullable: false),
                    BandLocationLeaderId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandLocationLeaderPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaderPeriods_BandLocationLeaders_BandLocationLeaderId",
                        column: x => x.BandLocationLeaderId,
                        principalTable: "BandLocationLeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaderPeriods_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BandLocationLeaderPeriodId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_BandLocationLeaderPeriods_BandLocationLeaderPeriodId",
                        column: x => x.BandLocationLeaderPeriodId,
                        principalTable: "BandLocationLeaderPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BandLocationLeaderPeriodEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    BandLocationLeaderPeriodId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandLocationLeaderPeriodEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaderPeriodEmployees_BandLocationLeaderPeriods_BandLocationLeaderPeriodId",
                        column: x => x.BandLocationLeaderPeriodId,
                        principalTable: "BandLocationLeaderPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaderPeriodEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BandLocationLeaderPeriodId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_BandLocationLeaderPeriods_BandLocationLeaderPeriodId",
                        column: x => x.BandLocationLeaderPeriodId,
                        principalTable: "BandLocationLeaderPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Westeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BandLocationLeaderPeriodId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Westeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Westeds_BandLocationLeaderPeriods_BandLocationLeaderPeriodId",
                        column: x => x.BandLocationLeaderPeriodId,
                        principalTable: "BandLocationLeaderPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BandLocationLeaderPeriodEmployeePeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDays = table.Column<double>(type: "float", nullable: false),
                    TotalSalary = table.Column<double>(type: "float", nullable: false),
                    TotalBorrow = table.Column<double>(type: "float", nullable: false),
                    TotalRemainder = table.Column<double>(type: "float", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    PayiedState = table.Column<bool>(type: "bit", nullable: false),
                    BandLocationLeaderPeriodEmployeeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandLocationLeaderPeriodEmployeePeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaderPeriodEmployeePeriods_BandLocationLeaderPeriodEmployees_BandLocationLeaderPeriodEmployeeId",
                        column: x => x.BandLocationLeaderPeriodEmployeeId,
                        principalTable: "BandLocationLeaderPeriodEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BandLocationLeaderPeriodEmployeePeriodAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceId = table.Column<int>(type: "int", nullable: false),
                    BandLocationLeaderPeriodEmployeePeriodId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    BorrowValue = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandLocationLeaderPeriodEmployeePeriodAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaderPeriodEmployeePeriodAttendances_Attendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "Attendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BandLocationLeaderPeriodEmployeePeriodAttendances_BandLocationLeaderPeriodEmployeePeriods_BandLocationLeaderPeriodEmployeePe~",
                        column: x => x.BandLocationLeaderPeriodEmployeePeriodId,
                        principalTable: "BandLocationLeaderPeriodEmployeePeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_BandLocationLeaderPeriodId",
                table: "Attendances",
                column: "BandLocationLeaderPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaderPeriodEmployeePeriodAttendances_AttendanceId",
                table: "BandLocationLeaderPeriodEmployeePeriodAttendances",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaderPeriodEmployeePeriodAttendances_BandLocationLeaderPeriodEmployeePeriodId",
                table: "BandLocationLeaderPeriodEmployeePeriodAttendances",
                column: "BandLocationLeaderPeriodEmployeePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaderPeriodEmployeePeriods_BandLocationLeaderPeriodEmployeeId",
                table: "BandLocationLeaderPeriodEmployeePeriods",
                column: "BandLocationLeaderPeriodEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaderPeriodEmployees_BandLocationLeaderPeriodId",
                table: "BandLocationLeaderPeriodEmployees",
                column: "BandLocationLeaderPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaderPeriodEmployees_EmployeeId",
                table: "BandLocationLeaderPeriodEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaderPeriods_BandLocationLeaderId",
                table: "BandLocationLeaderPeriods",
                column: "BandLocationLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaderPeriods_PeriodId",
                table: "BandLocationLeaderPeriods",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaders_BandId",
                table: "BandLocationLeaders",
                column: "BandId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaders_BandLocationId",
                table: "BandLocationLeaders",
                column: "BandLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLocationLeaders_LeaderId",
                table: "BandLocationLeaders",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BandLocationId",
                table: "Bills",
                column: "BandLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BLWesteds_BandLocationId",
                table: "BLWesteds",
                column: "BandLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TypeId",
                table: "Employees",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_BandLocationId",
                table: "Incomes",
                column: "BandLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_BandLocationId",
                table: "Periods",
                column: "BandLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BandLocationLeaderPeriodId",
                table: "Transactions",
                column: "BandLocationLeaderPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Westeds_BandLocationLeaderPeriodId",
                table: "Westeds",
                column: "BandLocationLeaderPeriodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandLocationLeaderPeriodEmployeePeriodAttendances");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "BLWesteds");

            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Westeds");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "BandLocationLeaderPeriodEmployeePeriods");

            migrationBuilder.DropTable(
                name: "BandLocationLeaderPeriodEmployees");

            migrationBuilder.DropTable(
                name: "BandLocationLeaderPeriods");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "BandLocationLeaders");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "EmployeeTypes");

            migrationBuilder.DropTable(
                name: "Leaders");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "BandCompanies",
                columns: table => new
                {
                    BandCompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BandId = table.Column<int>(type: "int", nullable: false),
                    BusinessId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandCompanies", x => x.BandCompanyId);
                    table.ForeignKey(
                        name: "FK_BandCompanies_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BandCompanies_BandId",
                table: "BandCompanies",
                column: "BandId");

            migrationBuilder.CreateIndex(
                name: "IX_BandCompanies_CompanyId",
                table: "BandCompanies",
                column: "CompanyId");
        }
    }
}
