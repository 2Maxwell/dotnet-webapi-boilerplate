using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class CancellationPolicy_Update_DisplayStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayShort",
                schema: "Lnx",
                table: "CancellationPolicy",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "ChipIcon",
                schema: "Lnx",
                table: "CancellationPolicy",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChipText",
                schema: "Lnx",
                table: "CancellationPolicy",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "CancellationPolicy",
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
                table: "CancellationPolicy");

            migrationBuilder.DropColumn(
                name: "ChipText",
                schema: "Lnx",
                table: "CancellationPolicy");

            migrationBuilder.DropColumn(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "CancellationPolicy");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayShort",
                schema: "Lnx",
                table: "CancellationPolicy",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
        }
    }
}
