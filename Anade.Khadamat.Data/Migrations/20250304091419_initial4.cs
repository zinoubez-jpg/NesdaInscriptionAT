using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionFormations");

            migrationBuilder.CreateTable(
                name: "SessionFormationsDetails",
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
                    table.PrimaryKey("PK_SessionFormationsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionFormationsDetails_Formations_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionFormationsDetails_Inscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "Inscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionFormationsDetails_FormationId",
                table: "SessionFormationsDetails",
                column: "FormationId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionFormationsDetails_InscriptionId",
                table: "SessionFormationsDetails",
                column: "InscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionFormationsDetails");

            migrationBuilder.CreateTable(
                name: "SessionFormations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FormationId = table.Column<int>(type: "int", nullable: false),
                    InscriptionId = table.Column<int>(type: "int", nullable: false),
                    IsFormee = table.Column<bool>(type: "bit", nullable: false)
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
    }
}
