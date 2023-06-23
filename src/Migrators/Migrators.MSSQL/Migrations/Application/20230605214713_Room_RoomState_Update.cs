using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Room_RoomState_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ArrCi",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ArrExp",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Assigned",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AssignedId",
                schema: "Lnx",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CleanChecked",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DepExp",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DepOut",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DirtyDays",
                schema: "Lnx",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCleaning",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MinutesDefault",
                schema: "Lnx",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesDeparture",
                schema: "Lnx",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesOccupied",
                schema: "Lnx",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Occ",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrCi",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "ArrExp",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Assigned",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "AssignedId",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "CleanChecked",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "DepExp",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "DepOut",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "DirtyDays",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "IsCleaning",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "MinutesDefault",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "MinutesDeparture",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "MinutesOccupied",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Occ",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "PendingClean",
                schema: "Lnx",
                table: "Room");
        }
    }
}
