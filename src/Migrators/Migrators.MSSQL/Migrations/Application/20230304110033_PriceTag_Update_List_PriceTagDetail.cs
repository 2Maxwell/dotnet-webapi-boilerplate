using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class PriceTag_Update_List_PriceTagDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceTagDetail_PriceTagId",
                schema: "Lnx",
                table: "PriceTagDetail");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTagDetail_PriceTagId",
                schema: "Lnx",
                table: "PriceTagDetail",
                column: "PriceTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceTagDetail_PriceTagId",
                schema: "Lnx",
                table: "PriceTagDetail");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTagDetail_PriceTagId",
                schema: "Lnx",
                table: "PriceTagDetail",
                column: "PriceTagId",
                unique: true);
        }
    }
}
