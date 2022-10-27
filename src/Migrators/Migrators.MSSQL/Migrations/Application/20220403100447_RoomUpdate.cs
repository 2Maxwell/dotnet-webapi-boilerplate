using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class RoomUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Blocked",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "BlockedEnd",
                schema: "Lnx",
                table: "Room",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BlockedStart",
                schema: "Lnx",
                table: "Room",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Clean",
                schema: "Lnx",
                table: "Room",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blocked",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "BlockedEnd",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "BlockedStart",
                schema: "Lnx",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Clean",
                schema: "Lnx",
                table: "Room");
        }
    }
}
