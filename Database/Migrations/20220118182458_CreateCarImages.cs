using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class CreateCarImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarImage",
                columns: table => new
                {
                    ImageName = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    CarPlate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarImage", x => x.ImageName);
                    table.ForeignKey(
                        name: "FK_CarImage_Cars_CarPlate",
                        column: x => x.CarPlate,
                        principalTable: "Cars",
                        principalColumn: "Plate");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarImage_CarPlate",
                table: "CarImage",
                column: "CarPlate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarImage");
        }
    }
}
