using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Invoice_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoice",
                schema: "Lnx",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MandantId = table.Column<int>(type: "int", nullable: false),
                    InvoiceIdMandant = table.Column<int>(type: "int", nullable: false),
                    CreditId = table.Column<int>(type: "int", nullable: true),
                    ReservationId = table.Column<int>(type: "int", nullable: true),
                    BookerId = table.Column<int>(type: "int", nullable: true),
                    GuestId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CompanyContactId = table.Column<int>(type: "int", nullable: true),
                    TravelAgentId = table.Column<int>(type: "int", nullable: true),
                    TravelAgentContactId = table.Column<int>(type: "int", nullable: true),
                    HotelDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCurrent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    InvoiceAddressSource = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<int>(type: "int", nullable: true),
                    InvoiceTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceTotalNet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceTaxes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InvoicePayments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InvoicePosition = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    InvoiceKz = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoice",
                schema: "Lnx");
        }
    }
}
