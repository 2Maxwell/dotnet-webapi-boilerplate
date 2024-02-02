using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Package_Update_Duration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                schema: "Lnx",
                table: "Package",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationUnitEnumId",
                schema: "Lnx",
                table: "Package",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "DurationUnitEnumId",
                schema: "Lnx",
                table: "Package");
        }
    }
}
