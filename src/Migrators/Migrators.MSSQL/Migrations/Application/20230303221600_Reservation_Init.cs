using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Reservation_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UserRate",
                schema: "Lnx",
                table: "PriceTagDetail",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "Reservation",
                schema: "Lnx",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MandantId = table.Column<int>(type: "int", nullable: false),
                    ResKz = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    BookerId = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CompanyContactId = table.Column<int>(type: "int", nullable: true),
                    TravelAgentId = table.Column<int>(type: "int", nullable: true),
                    TravelAgentContactId = table.Column<int>(type: "int", nullable: true),
                    Persons = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Arrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Departure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    RoomAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomNumberId = table.Column<int>(type: "int", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    RoomFixed = table.Column<bool>(type: "bit", nullable: false),
                    RateId = table.Column<int>(type: "int", nullable: false),
                    LogisTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingPolicyId = table.Column<int>(type: "int", nullable: false),
                    CancellationPolicyId = table.Column<int>(type: "int", nullable: false),
                    IsGroupMaster = table.Column<bool>(type: "bit", nullable: false),
                    GroupMasterId = table.Column<int>(type: "int", nullable: false),
                    Transfer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MatchCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    OptionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OptionFollowUp = table.Column<int>(type: "int", nullable: false),
                    CRSNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PaxString = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Confirmations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wishes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Lnx",
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservation_Company_TravelAgentId",
                        column: x => x.TravelAgentId,
                        principalSchema: "Lnx",
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservation_Person_BookerId",
                        column: x => x.BookerId,
                        principalSchema: "Lnx",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Person_CompanyContactId",
                        column: x => x.CompanyContactId,
                        principalSchema: "Lnx",
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservation_Person_GuestId",
                        column: x => x.GuestId,
                        principalSchema: "Lnx",
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservation_Person_TravelAgentContactId",
                        column: x => x.TravelAgentContactId,
                        principalSchema: "Lnx",
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_BookerId",
                schema: "Lnx",
                table: "Reservation",
                column: "BookerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_CompanyContactId",
                schema: "Lnx",
                table: "Reservation",
                column: "CompanyContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_CompanyId",
                schema: "Lnx",
                table: "Reservation",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_GuestId",
                schema: "Lnx",
                table: "Reservation",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TravelAgentContactId",
                schema: "Lnx",
                table: "Reservation",
                column: "TravelAgentContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TravelAgentId",
                schema: "Lnx",
                table: "Reservation",
                column: "TravelAgentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservation",
                schema: "Lnx");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserRate",
                schema: "Lnx",
                table: "PriceTagDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
