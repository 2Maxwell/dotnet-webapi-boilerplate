using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Journal_Booking_Update_KasseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoicePosition",
                schema: "Lnx",
                table: "Invoice");

            migrationBuilder.AddColumn<int>(
                name: "KasseId",
                schema: "Lnx",
                table: "Booking",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KasseId",
                schema: "Lnx",
                table: "Booking");

            migrationBuilder.AddColumn<int>(
                name: "InvoicePosition",
                schema: "Lnx",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
