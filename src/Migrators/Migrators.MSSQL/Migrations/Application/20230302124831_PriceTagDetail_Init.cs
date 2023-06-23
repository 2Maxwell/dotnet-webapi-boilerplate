using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class PriceTagDetail_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HotelDate",
                schema: "Lnx",
                table: "Mandant",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TaxCountryId",
                schema: "Lnx",
                table: "Mandant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PriceTagDetail",
                schema: "Lnx",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceTagId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    RateId = table.Column<int>(type: "int", nullable: false),
                    DatePrice = table.Column<DateTime>(type: "Date", nullable: false),
                    PaxAmount = table.Column<int>(type: "int", nullable: false),
                    RateCurrent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateStart = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateAutomatic = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EventDates = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RateTypeEnumId = table.Column<int>(type: "int", nullable: false),
                    NoShow = table.Column<bool>(type: "bit", nullable: false),
                    NoShowPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTagDetail", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceTagDetail",
                schema: "Lnx");

            migrationBuilder.DropColumn(
                name: "HotelDate",
                schema: "Lnx",
                table: "Mandant");

            migrationBuilder.DropColumn(
                name: "TaxCountryId",
                schema: "Lnx",
                table: "Mandant");
        }
    }
}
