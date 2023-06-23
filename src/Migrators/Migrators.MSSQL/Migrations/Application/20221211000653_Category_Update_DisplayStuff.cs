using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Category_Update_DisplayStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayHightlights",
                schema: "Lnx",
                table: "Category",
                newName: "DisplayHighLight");

            migrationBuilder.RenameColumn(
                name: "DisplayDescriptionShort",
                schema: "Lnx",
                table: "Category",
                newName: "DisplayShort");

            migrationBuilder.AddColumn<string>(
                name: "ChipIcon",
                schema: "Lnx",
                table: "Category",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChipText",
                schema: "Lnx",
                table: "Category",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Display",
                schema: "Lnx",
                table: "Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChipIcon",
                schema: "Lnx",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ChipText",
                schema: "Lnx",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Display",
                schema: "Lnx",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "DisplayShort",
                schema: "Lnx",
                table: "Category",
                newName: "DisplayDescriptionShort");

            migrationBuilder.RenameColumn(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "Category",
                newName: "DisplayHightlights");
        }
    }
}
