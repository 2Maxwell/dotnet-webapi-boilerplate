using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Invoice_Update_JsonNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceTaxes",
                schema: "Lnx",
                table: "Invoice",
                newName: "InvoiceTaxesJson");

            migrationBuilder.RenameColumn(
                name: "InvoicePayments",
                schema: "Lnx",
                table: "Invoice",
                newName: "InvoicePaymentsJson");

            migrationBuilder.RenameColumn(
                name: "InvoiceAddress",
                schema: "Lnx",
                table: "Invoice",
                newName: "InvoiceAddressJson");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceTaxesJson",
                schema: "Lnx",
                table: "Invoice",
                newName: "InvoiceTaxes");

            migrationBuilder.RenameColumn(
                name: "InvoicePaymentsJson",
                schema: "Lnx",
                table: "Invoice",
                newName: "InvoicePayments");

            migrationBuilder.RenameColumn(
                name: "InvoiceAddressJson",
                schema: "Lnx",
                table: "Invoice",
                newName: "InvoiceAddress");
        }
    }
}
