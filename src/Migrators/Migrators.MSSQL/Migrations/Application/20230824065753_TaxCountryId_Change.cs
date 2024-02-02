using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class TaxCountryId_Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxCountryId",
                schema: "Lnx",
                table: "Mandant");

            migrationBuilder.AddColumn<int>(
                name: "TaxCountryId",
                schema: "Lnx",
                table: "MandantSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxCountryId",
                schema: "Lnx",
                table: "MandantSetting");

            migrationBuilder.AddColumn<int>(
                name: "TaxCountryId",
                schema: "Lnx",
                table: "Mandant",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
