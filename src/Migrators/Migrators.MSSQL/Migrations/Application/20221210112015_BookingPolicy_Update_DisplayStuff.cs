using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class BookingPolicy_Update_DisplayStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PackageTargetEnum",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Display",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayShort",
                schema: "Lnx",
                table: "BookingPolicy",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "ChipIcon",
                schema: "Lnx",
                table: "BookingPolicy",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChipText",
                schema: "Lnx",
                table: "BookingPolicy",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "BookingPolicy",
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
                table: "BookingPolicy");

            migrationBuilder.DropColumn(
                name: "ChipText",
                schema: "Lnx",
                table: "BookingPolicy");

            migrationBuilder.DropColumn(
                name: "DisplayHighLight",
                schema: "Lnx",
                table: "BookingPolicy");

            migrationBuilder.AlterColumn<string>(
                name: "PackageTargetEnum",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Display",
                schema: "Lnx",
                table: "Package",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayShort",
                schema: "Lnx",
                table: "BookingPolicy",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
        }
    }
}
