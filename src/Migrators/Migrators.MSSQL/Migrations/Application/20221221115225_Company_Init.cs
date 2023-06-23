using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Company_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "Lnx",
                table: "Person",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "Lnx",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MandantId = table.Column<int>(type: "int", nullable: false),
                    Name1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    City = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    StateRegionId = table.Column<int>(type: "int", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Telefax = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Mobil = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    EmailInvoice = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    WebSite = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: true),
                    CompanyTypEnumId = table.Column<int>(type: "int", nullable: true),
                    CommissionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AddTax = table.Column<bool>(type: "bit", nullable: true),
                    GroupHeadId = table.Column<int>(type: "int", nullable: true),
                    PriceContractId = table.Column<int>(type: "int", nullable: true),
                    Kz = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company",
                schema: "Lnx");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "Lnx",
                table: "Person",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70,
                oldNullable: true);
        }
    }
}
