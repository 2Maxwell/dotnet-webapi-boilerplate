using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Journal_Update_InvoiceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                schema: "Lnx",
                table: "Journal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceIdMandant",
                schema: "Lnx",
                table: "Journal",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceId",
                schema: "Lnx",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "InvoiceIdMandant",
                schema: "Lnx",
                table: "Journal");
        }
    }
}
