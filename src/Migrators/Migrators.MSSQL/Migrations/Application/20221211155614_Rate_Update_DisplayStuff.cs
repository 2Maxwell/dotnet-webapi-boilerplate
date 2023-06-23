using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Rate_Update_DisplayStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayShort",
                schema: "Lnx",
                table: "Rate",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "ChipIcon",
                schema: "Lnx",
                table: "Rate",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChipText",
                schema: "Lnx",
                table: "Rate",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationText",
                schema: "Lnx",
                table: "Rate",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "Rate",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayShort",
                schema: "Lnx",
                table: "Category",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "Category",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationText",
                schema: "Lnx",
                table: "Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChipIcon",
                schema: "Lnx",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "ChipText",
                schema: "Lnx",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "ConfirmationText",
                schema: "Lnx",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "ConfirmationText",
                schema: "Lnx",
                table: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayShort",
                schema: "Lnx",
                table: "Rate",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayShort",
                schema: "Lnx",
                table: "Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);
        }
    }
}
