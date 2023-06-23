using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Person_Update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MandantID",
                schema: "Lnx",
                table: "Person",
                newName: "MandantId");

            migrationBuilder.RenameColumn(
                name: "Telephon",
                schema: "Lnx",
                table: "Person",
                newName: "Telephone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MandantId",
                schema: "Lnx",
                table: "Person",
                newName: "MandantID");

            migrationBuilder.RenameColumn(
                name: "Telephone",
                schema: "Lnx",
                table: "Person",
                newName: "Telephon");
        }
    }
}
