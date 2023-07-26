using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Journal_Update_Kasse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KasseId",
                schema: "Lnx",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "BookingIdMandant",
                schema: "Lnx",
                table: "Journal",
                newName: "BookingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingId",
                schema: "Lnx",
                table: "Journal",
                newName: "BookingIdMandant");

            migrationBuilder.AddColumn<int>(
                name: "KasseId",
                schema: "Lnx",
                table: "Booking",
                type: "int",
                nullable: true);
        }
    }
}
