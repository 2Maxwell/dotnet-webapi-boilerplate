using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Package_Update_DisplayStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Display",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ChipIcon",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChipText",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationText",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DisplayShort",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChipIcon",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "ChipText",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "ConfirmationText",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "DisplayShort",
                schema: "Lnx",
                table: "Package");

            migrationBuilder.AlterColumn<string>(
                name: "Display",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
