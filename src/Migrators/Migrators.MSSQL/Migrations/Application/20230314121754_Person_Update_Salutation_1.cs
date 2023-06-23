using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Person_Update_Salutation_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Salutation_SalutationId",
                schema: "Lnx",
                table: "Person");

            migrationBuilder.AlterColumn<int>(
                name: "SalutationId",
                schema: "Lnx",
                table: "Person",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Salutation_SalutationId",
                schema: "Lnx",
                table: "Person",
                column: "SalutationId",
                principalSchema: "Lnx",
                principalTable: "Salutation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Salutation_SalutationId",
                schema: "Lnx",
                table: "Person");

            migrationBuilder.AlterColumn<int>(
                name: "SalutationId",
                schema: "Lnx",
                table: "Person",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Salutation_SalutationId",
                schema: "Lnx",
                table: "Person",
                column: "SalutationId",
                principalSchema: "Lnx",
                principalTable: "Salutation",
                principalColumn: "Id");
        }
    }
}
