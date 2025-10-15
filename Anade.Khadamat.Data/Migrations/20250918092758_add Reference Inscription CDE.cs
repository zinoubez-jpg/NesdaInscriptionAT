using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class addReferenceInscriptionCDE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CentreDeveloppementEntrepreneuriatId",
                table: "Inscriptions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_CentreDeveloppementEntrepreneuriatId",
                table: "Inscriptions",
                column: "CentreDeveloppementEntrepreneuriatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_CentreDeveloppementEntrepreneuriat_CentreDeveloppementEntrepreneuriatId",
                table: "Inscriptions",
                column: "CentreDeveloppementEntrepreneuriatId",
                principalTable: "CentreDeveloppementEntrepreneuriat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_CentreDeveloppementEntrepreneuriat_CentreDeveloppementEntrepreneuriatId",
                table: "Inscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Inscriptions_CentreDeveloppementEntrepreneuriatId",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "CentreDeveloppementEntrepreneuriatId",
                table: "Inscriptions");
        }
    }
}
