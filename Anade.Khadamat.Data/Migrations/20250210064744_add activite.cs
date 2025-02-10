using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class addactivite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiviteId",
                table: "Inscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Banque",
                table: "Inscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FormJuridique",
                table: "Inscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FrnchisseTVA",
                table: "Inscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MontantPNR",
                table: "Inscriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "NIF",
                table: "Inscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumComtBancaire",
                table: "Inscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RCAGCFCA",
                table: "Inscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RegimFiscal",
                table: "Inscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AgenceBancaire",
                table: "CSVFs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Banque",
                table: "CSVFs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Activites",
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
                    table.PrimaryKey("PK_Activites", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_ActiviteId",
                table: "Inscriptions",
                column: "ActiviteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Activites_ActiviteId",
                table: "Inscriptions",
                column: "ActiviteId",
                principalTable: "Activites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Activites_ActiviteId",
                table: "Inscriptions");

            migrationBuilder.DropTable(
                name: "Activites");

            migrationBuilder.DropIndex(
                name: "IX_Inscriptions_ActiviteId",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "ActiviteId",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "Banque",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "FormJuridique",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "FrnchisseTVA",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "MontantPNR",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "NIF",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "NumComtBancaire",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "RCAGCFCA",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "RegimFiscal",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "AgenceBancaire",
                table: "CSVFs");

            migrationBuilder.DropColumn(
                name: "Banque",
                table: "CSVFs");
        }
    }
}
