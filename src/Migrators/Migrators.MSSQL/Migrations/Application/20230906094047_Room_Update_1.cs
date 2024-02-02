using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Room_Update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameUnique",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "PhoneNumberUnique",
                schema: "Lnx",
                table: "Room");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameUnique",
                schema: "Lnx",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberUnique",
                schema: "Lnx",
                table: "Room",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
