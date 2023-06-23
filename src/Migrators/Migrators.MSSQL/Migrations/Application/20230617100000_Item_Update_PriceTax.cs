using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Item_Update_PriceTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                schema: "Lnx",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "TaxId",
                schema: "Lnx",
                table: "Item",
                newName: "InvoicePosition");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                schema: "Lnx",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                schema: "Lnx",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "InvoicePosition",
                schema: "Lnx",
                table: "Item",
                newName: "TaxId");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "Lnx",
                table: "Item",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
