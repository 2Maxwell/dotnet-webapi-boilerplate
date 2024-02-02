using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Board_Update_Source : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DefaultBoardItemLabelGroupId",
                schema: "Lnx",
                table: "Board",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                schema: "Lnx",
                table: "Board",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                schema: "Lnx",
                table: "Board",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                schema: "Lnx",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "SourceId",
                schema: "Lnx",
                table: "Board");

            migrationBuilder.AlterColumn<int>(
                name: "DefaultBoardItemLabelGroupId",
                schema: "Lnx",
                table: "Board",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
