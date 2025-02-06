using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DesignationFr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DesignationAr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NiveauInstruction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DesignationFr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DesignationAr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NiveauInstruction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SituationFamiliales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DesignationFr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DesignationAr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituationFamiliales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeDecisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DesignationFr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DesignationAr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeDecisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypesInscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DesignationFr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DesignationAr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesInscription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wilayas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DesignationFr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DesignationAr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wilayas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Communes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WilayaId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DesignationFr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DesignationAr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communes_Wilayas_WilayaId",
                        column: x => x.WilayaId,
                        principalTable: "Wilayas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Inscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeInscriptionId = table.Column<int>(type: "int", nullable: false),
                    DateInscription = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NIN = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    NomFr = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PrenomFr = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    NomAr = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PrenomAr = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WilayaNaissanceId = table.Column<int>(type: "int", nullable: false),
                    CommuneNaissanceId = table.Column<int>(type: "int", nullable: false),
                    SituationFamilialeId = table.Column<int>(type: "int", nullable: false),
                    NomJeunneFille = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomJeunneFilleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdressAr = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AdressFr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WilayaResidenceId = table.Column<int>(type: "int", nullable: false),
                    CommuneResidenceId = table.Column<int>(type: "int", nullable: false),
                    NumTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NiveauInstructionId = table.Column<int>(type: "int", nullable: false),
                    EtablissementFormation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntituleDiplome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WilayaSeigeSocialId = table.Column<int>(type: "int", nullable: false),
                    CommuneSeigeSocialId = table.Column<int>(type: "int", nullable: false),
                    AddresseArSeigeSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddresseFrSeigeSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Validee = table.Column<bool>(type: "bit", nullable: false),
                    ValideePAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateValidation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Annulee = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    EmailVerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailVerificationTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecaptchaResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CopieCNIPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopieDiplomeRCCFCAPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Communes_CommuneNaissanceId",
                        column: x => x.CommuneNaissanceId,
                        principalTable: "Communes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inscriptions_Communes_CommuneResidenceId",
                        column: x => x.CommuneResidenceId,
                        principalTable: "Communes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inscriptions_Communes_CommuneSeigeSocialId",
                        column: x => x.CommuneSeigeSocialId,
                        principalTable: "Communes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inscriptions_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inscriptions_NiveauInstruction_NiveauInstructionId",
                        column: x => x.NiveauInstructionId,
                        principalTable: "NiveauInstruction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inscriptions_SituationFamiliales_SituationFamilialeId",
                        column: x => x.SituationFamilialeId,
                        principalTable: "SituationFamiliales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inscriptions_TypesInscription_TypeInscriptionId",
                        column: x => x.TypeInscriptionId,
                        principalTable: "TypesInscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CSVFs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultatCSVF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCSVF = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValide = table.Column<bool>(type: "bit", nullable: false),
                    InscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSVFs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CSVFs_Inscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "Inscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Decisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeDecision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeDecisionId = table.Column<int>(type: "int", nullable: false),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false),
                    DateEtablissementAE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decisions_Inscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "Inscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Decisions_TypeDecisions_TypeDecisionId",
                        column: x => x.TypeDecisionId,
                        principalTable: "TypeDecisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationProjets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeFiche = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScoreTotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmis = table.Column<bool>(type: "bit", nullable: false),
                    DateEvaluation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationProjets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationProjets_Inscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "Inscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Formations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    libelleFormation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFormee = table.Column<bool>(type: "bit", nullable: false),
                    InscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Formations_Inscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "Inscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communes_WilayaId",
                table: "Communes",
                column: "WilayaId");

            migrationBuilder.CreateIndex(
                name: "IX_CSVFs_InscriptionId",
                table: "CSVFs",
                column: "InscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_InscriptionId",
                table: "Decisions",
                column: "InscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_TypeDecisionId",
                table: "Decisions",
                column: "TypeDecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationProjets_InscriptionId",
                table: "EvaluationProjets",
                column: "InscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Formations_InscriptionId",
                table: "Formations",
                column: "InscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_CommuneNaissanceId",
                table: "Inscriptions",
                column: "CommuneNaissanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_CommuneResidenceId",
                table: "Inscriptions",
                column: "CommuneResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_CommuneSeigeSocialId",
                table: "Inscriptions",
                column: "CommuneSeigeSocialId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_GenreId",
                table: "Inscriptions",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_NiveauInstructionId",
                table: "Inscriptions",
                column: "NiveauInstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_SituationFamilialeId",
                table: "Inscriptions",
                column: "SituationFamilialeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_TypeInscriptionId",
                table: "Inscriptions",
                column: "TypeInscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSVFs");

            migrationBuilder.DropTable(
                name: "Decisions");

            migrationBuilder.DropTable(
                name: "EvaluationProjets");

            migrationBuilder.DropTable(
                name: "Formations");

            migrationBuilder.DropTable(
                name: "TypeDecisions");

            migrationBuilder.DropTable(
                name: "Inscriptions");

            migrationBuilder.DropTable(
                name: "Communes");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "NiveauInstruction");

            migrationBuilder.DropTable(
                name: "SituationFamiliales");

            migrationBuilder.DropTable(
                name: "TypesInscription");

            migrationBuilder.DropTable(
                name: "Wilayas");
        }
    }
}
