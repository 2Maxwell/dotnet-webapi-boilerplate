using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class BoardItemSub_Update_Add_ValuePropertys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                schema: "Lnx",
                table: "BoardItemSub",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ResultValueBool",
                schema: "Lnx",
                table: "BoardItemSub",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ResultValueDecimal",
                schema: "Lnx",
                table: "BoardItemSub",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResultValueInt",
                schema: "Lnx",
                table: "BoardItemSub",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultValueString",
                schema: "Lnx",
                table: "BoardItemSub",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                schema: "Lnx",
                table: "BoardItemSub");

            migrationBuilder.DropColumn(
                name: "ResultValueBool",
                schema: "Lnx",
                table: "BoardItemSub");

            migrationBuilder.DropColumn(
                name: "ResultValueDecimal",
                schema: "Lnx",
                table: "BoardItemSub");

            migrationBuilder.DropColumn(
                name: "ResultValueInt",
                schema: "Lnx",
                table: "BoardItemSub");

            migrationBuilder.DropColumn(
                name: "ResultValueString",
                schema: "Lnx",
                table: "BoardItemSub");
        }
    }
}
