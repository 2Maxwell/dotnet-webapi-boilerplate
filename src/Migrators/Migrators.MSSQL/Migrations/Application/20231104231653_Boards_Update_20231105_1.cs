using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Boards_Update_20231105_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoardItemAttachmentIds",
                schema: "Lnx",
                table: "BoardItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BoardItemAttachmentIds",
                schema: "Lnx",
                table: "BoardItem",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);
        }
    }
}
