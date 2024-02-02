using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class PackageExtend_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Appointment",
                schema: "Lnx",
                table: "PackageExtend");

            migrationBuilder.DropColumn(
                name: "AppointmentSource",
                schema: "Lnx",
                table: "PackageExtend");

            migrationBuilder.DropColumn(
                name: "AppointmentSourceId",
                schema: "Lnx",
                table: "PackageExtend");

            migrationBuilder.RenameColumn(
                name: "Duration",
                schema: "Lnx",
                table: "PackageExtend",
                newName: "AppointmentId");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentTargetEnum",
                schema: "Lnx",
                table: "PackageExtend",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentTargetEnum",
                schema: "Lnx",
                table: "PackageExtend");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                schema: "Lnx",
                table: "PackageExtend",
                newName: "Duration");

            migrationBuilder.AddColumn<DateTime>(
                name: "Appointment",
                schema: "Lnx",
                table: "PackageExtend",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentSource",
                schema: "Lnx",
                table: "PackageExtend",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentSourceId",
                schema: "Lnx",
                table: "PackageExtend",
                type: "int",
                nullable: true);
        }
    }
}
