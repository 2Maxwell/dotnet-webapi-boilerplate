using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Room_Update_CleaningState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assigned",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "CleanChecked",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "IsCleaning",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "PendingClean",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.AddColumn<int>(
                name: "CleaningState",
                schema: "Lnx",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CleaningState",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.AddColumn<bool>(
                name: "Assigned",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CleanChecked",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCleaning",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PendingClean",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
