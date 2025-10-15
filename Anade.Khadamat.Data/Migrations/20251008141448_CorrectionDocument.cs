using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class CorrectionDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Document",
                newName: "FilePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Document",
                newName: "Nom");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Document",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
