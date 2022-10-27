using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class PriceCatRate_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Events",
                schema: "Lnx",
                table: "PriceCatRate",
                newName: "EventDates");

            migrationBuilder.CreateIndex(
                name: "IX_PriceCatRate_CategoryId",
                schema: "Lnx",
                table: "PriceCatRate",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceCatRate_RateId",
                schema: "Lnx",
                table: "PriceCatRate",
                column: "RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceCatRate_Category_CategoryId",
                schema: "Lnx",
                table: "PriceCatRate",
                column: "CategoryId",
                principalSchema: "Lnx",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceCatRate_Rate_RateId",
                schema: "Lnx",
                table: "PriceCatRate",
                column: "RateId",
                principalSchema: "Lnx",
                principalTable: "Rate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceCatRate_Category_CategoryId",
                schema: "Lnx",
                table: "PriceCatRate");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceCatRate_Rate_RateId",
                schema: "Lnx",
                table: "PriceCatRate");

            migrationBuilder.DropIndex(
                name: "IX_PriceCatRate_CategoryId",
                schema: "Lnx",
                table: "PriceCatRate");

            migrationBuilder.DropIndex(
                name: "IX_PriceCatRate_RateId",
                schema: "Lnx",
                table: "PriceCatRate");

            migrationBuilder.RenameColumn(
                name: "EventDates",
                schema: "Lnx",
                table: "PriceCatRate",
                newName: "Events");
        }
    }
}
