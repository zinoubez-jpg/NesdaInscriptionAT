using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class changementcodeantenneagence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CodeAnntenne",
                table: "Inscriptions",
                newName: "Anntenne");

            migrationBuilder.RenameColumn(
                name: "CodeAgence",
                table: "Inscriptions",
                newName: "Agence");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Anntenne",
                table: "Inscriptions",
                newName: "CodeAnntenne");

            migrationBuilder.RenameColumn(
                name: "Agence",
                table: "Inscriptions",
                newName: "CodeAgence");
        }
    }
}
