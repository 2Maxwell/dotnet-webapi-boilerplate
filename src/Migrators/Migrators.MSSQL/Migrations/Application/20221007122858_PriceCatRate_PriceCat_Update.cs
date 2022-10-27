using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class PriceCatRate_PriceCat_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceCatRate",
                schema: "Lnx");

            migrationBuilder.CreateTable(
                name: "PriceCat",
                schema: "Lnx",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MandantId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pax = table.Column<int>(type: "int", nullable: false),
                    RateStart = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateAutomatic = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EventDates = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RateTypeEnumId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceCat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceCat_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Lnx",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceCat_CategoryId",
                schema: "Lnx",
                table: "PriceCat",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceCat",
                schema: "Lnx");

            migrationBuilder.CreateTable(
                name: "PriceCatRate",
                schema: "Lnx",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    RateId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventDates = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MandantId = table.Column<int>(type: "int", nullable: false),
                    Pax = table.Column<int>(type: "int", nullable: false),
                    RateAutomatic = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateCurrent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateStart = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateTypeEnumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceCatRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceCatRate_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Lnx",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceCatRate_Rate_RateId",
                        column: x => x.RateId,
                        principalSchema: "Lnx",
                        principalTable: "Rate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
        }
    }
}
