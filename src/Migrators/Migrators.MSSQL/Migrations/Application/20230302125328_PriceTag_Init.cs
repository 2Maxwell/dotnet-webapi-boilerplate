using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class PriceTag_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceTag",
                schema: "Lnx",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MandantId = table.Column<int>(type: "int", nullable: false),
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    Arrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Departure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AverageRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RateSelected = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTag", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceTagDetail_PriceTagId",
                schema: "Lnx",
                table: "PriceTagDetail",
                column: "PriceTagId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceTagDetail_PriceTag_PriceTagId",
                schema: "Lnx",
                table: "PriceTagDetail",
                column: "PriceTagId",
                principalSchema: "Lnx",
                principalTable: "PriceTag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceTagDetail_PriceTag_PriceTagId",
                schema: "Lnx",
                table: "PriceTagDetail");

            migrationBuilder.DropTable(
                name: "PriceTag",
                schema: "Lnx");

            migrationBuilder.DropIndex(
                name: "IX_PriceTagDetail_PriceTagId",
                schema: "Lnx",
                table: "PriceTagDetail");
        }
    }
}
