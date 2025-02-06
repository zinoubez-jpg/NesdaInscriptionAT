using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class addformationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formations_Inscriptions_InscriptionId",
                table: "Formations");

            migrationBuilder.DropIndex(
                name: "IX_Formations_InscriptionId",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "CodeSession",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "DateDebut",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "DateFin",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "InscriptionId",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "IsFormee",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "libelleFormation",
                table: "Formations");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Formations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DesignationAr",
                table: "Formations",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignationFr",
                table: "Formations",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SessionFormations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormationId = table.Column<int>(type: "int", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFormee = table.Column<bool>(type: "bit", nullable: false),
                    InscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionFormations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionFormations_Formations_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionFormations_Inscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "Inscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionFormations_FormationId",
                table: "SessionFormations",
                column: "FormationId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionFormations_InscriptionId",
                table: "SessionFormations",
                column: "InscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionFormations");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "DesignationAr",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "DesignationFr",
                table: "Formations");

            migrationBuilder.AddColumn<string>(
                name: "CodeSession",
                table: "Formations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebut",
                table: "Formations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFin",
                table: "Formations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "InscriptionId",
                table: "Formations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFormee",
                table: "Formations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "libelleFormation",
                table: "Formations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Formations_InscriptionId",
                table: "Formations",
                column: "InscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formations_Inscriptions_InscriptionId",
                table: "Formations",
                column: "InscriptionId",
                principalTable: "Inscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
