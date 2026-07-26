using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class sms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPhoneValidated",
                table: "Inscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtpCode",
                table: "Inscriptions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiresAt",
                table: "Inscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StatutsEnvoiSms",
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
                    table.PrimaryKey("PK_StatutsEnvoiSms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypesSms",
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
                    table.PrimaryKey("PK_TypesSms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsSents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumTelephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TypeSmsId = table.Column<int>(type: "int", nullable: false),
                    InscriptionId = table.Column<int>(type: "int", nullable: true),
                    DateEnvoi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatutEnvoiSmsId = table.Column<int>(type: "int", nullable: false),
                    ReponseApi = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DateReponse = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsSents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsSents_Inscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalTable: "Inscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmsSents_StatutsEnvoiSms_StatutEnvoiSmsId",
                        column: x => x.StatutEnvoiSmsId,
                        principalTable: "StatutsEnvoiSms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsSents_TypesSms_TypeSmsId",
                        column: x => x.TypeSmsId,
                        principalTable: "TypesSms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmsSents_InscriptionId",
                table: "SmsSents",
                column: "InscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsSents_StatutEnvoiSmsId",
                table: "SmsSents",
                column: "StatutEnvoiSmsId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsSents_TypeSmsId",
                table: "SmsSents",
                column: "TypeSmsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsSents");

            migrationBuilder.DropTable(
                name: "StatutsEnvoiSms");

            migrationBuilder.DropTable(
                name: "TypesSms");

            migrationBuilder.DropColumn(
                name: "IsPhoneValidated",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "OtpCode",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "OtpExpiresAt",
                table: "Inscriptions");
        }
    }
}
