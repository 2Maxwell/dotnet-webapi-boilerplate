using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Person_Update_Salutation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Person_SalutationId",
                schema: "Lnx",
                table: "Person",
                column: "SalutationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Salutation_SalutationId",
                schema: "Lnx",
                table: "Person",
                column: "SalutationId",
                principalSchema: "Lnx",
                principalTable: "Salutation",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Salutation_SalutationId",
                schema: "Lnx",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_SalutationId",
                schema: "Lnx",
                table: "Person");
        }
    }
}
