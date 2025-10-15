using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class SupprimerAttributsFichiersUploadés : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CopieCNIPath",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "CopieDiplomeCDEPath",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "CopieDiplomeRCCFCAPath",
                table: "Inscriptions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CopieCNIPath",
                table: "Inscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CopieDiplomeCDEPath",
                table: "Inscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CopieDiplomeRCCFCAPath",
                table: "Inscriptions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
