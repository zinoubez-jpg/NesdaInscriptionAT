using Microsoft.EntityFrameworkCore.Migrations;

namespace Anade.Khadamat.Data.Migrations
{
    public partial class addCentreDeveloppementEntrepreneuriat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeCentreDeveloppementEntrepreneuriat",
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
                    table.PrimaryKey("PK_TypeCentreDeveloppementEntrepreneuriat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CentreDeveloppementEntrepreneuriat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WilayaId = table.Column<int>(type: "int", nullable: false),
                    TypeCentreDeveloppementEntrepreneuriatId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DesignationFr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DesignationAr = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentreDeveloppementEntrepreneuriat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentreDeveloppementEntrepreneuriat_TypeCentreDeveloppementEntrepreneuriat_TypeCentreDeveloppementEntrepreneuriatId",
                        column: x => x.TypeCentreDeveloppementEntrepreneuriatId,
                        principalTable: "TypeCentreDeveloppementEntrepreneuriat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CentreDeveloppementEntrepreneuriat_Wilayas_WilayaId",
                        column: x => x.WilayaId,
                        principalTable: "Wilayas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CentreDeveloppementEntrepreneuriat_TypeCentreDeveloppementEntrepreneuriatId",
                table: "CentreDeveloppementEntrepreneuriat",
                column: "TypeCentreDeveloppementEntrepreneuriatId");

            migrationBuilder.CreateIndex(
                name: "IX_CentreDeveloppementEntrepreneuriat_WilayaId",
                table: "CentreDeveloppementEntrepreneuriat",
                column: "WilayaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CentreDeveloppementEntrepreneuriat");

            migrationBuilder.DropTable(
                name: "TypeCentreDeveloppementEntrepreneuriat");
        }
    }
}
