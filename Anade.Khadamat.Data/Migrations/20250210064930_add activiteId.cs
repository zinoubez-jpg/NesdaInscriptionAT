using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class addactiviteId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Activites_ActiviteId",
                table: "Inscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "ActiviteId",
                table: "Inscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Activites_ActiviteId",
                table: "Inscriptions",
                column: "ActiviteId",
                principalTable: "Activites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Activites_ActiviteId",
                table: "Inscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "ActiviteId",
                table: "Inscriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Activites_ActiviteId",
                table: "Inscriptions",
                column: "ActiviteId",
                principalTable: "Activites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
