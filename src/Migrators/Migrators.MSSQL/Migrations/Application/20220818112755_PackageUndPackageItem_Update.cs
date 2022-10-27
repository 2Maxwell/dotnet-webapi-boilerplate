using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class PackageUndPackageItem_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Formula",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "PackageBookingMechanicEnumId",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "PriceIsStatic",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.AddColumn<int>(
                name: "PackageItemCoreValueEnumId",
                schema: "Lnx",
                table: "PackageItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceName",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageItemCoreValueEnumId",
                schema: "Lnx",
                table: "PackageItem");

            migrationBuilder.DropColumn(
                name: "InvoiceName",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.AddColumn<string>(
                name: "Formula",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                schema: "Lnx",
                table: "Package",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PackageBookingMechanicEnumId",
                schema: "Lnx",
                table: "Package",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "Lnx",
                table: "Package",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "PriceIsStatic",
                schema: "Lnx",
                table: "Package",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
