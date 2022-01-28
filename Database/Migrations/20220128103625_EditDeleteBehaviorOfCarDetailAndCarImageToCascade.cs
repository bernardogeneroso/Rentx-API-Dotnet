using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class EditDeleteBehaviorOfCarDetailAndCarImageToCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarsDetails_Cars_Plate",
                table: "CarsDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CarsImages_Cars_Plate",
                table: "CarsImages");

            migrationBuilder.AddForeignKey(
                name: "FK_CarsDetails_Cars_Plate",
                table: "CarsDetails",
                column: "Plate",
                principalTable: "Cars",
                principalColumn: "Plate",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarsImages_Cars_Plate",
                table: "CarsImages",
                column: "Plate",
                principalTable: "Cars",
                principalColumn: "Plate",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarsDetails_Cars_Plate",
                table: "CarsDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CarsImages_Cars_Plate",
                table: "CarsImages");

            migrationBuilder.AddForeignKey(
                name: "FK_CarsDetails_Cars_Plate",
                table: "CarsDetails",
                column: "Plate",
                principalTable: "Cars",
                principalColumn: "Plate",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarsImages_Cars_Plate",
                table: "CarsImages",
                column: "Plate",
                principalTable: "Cars",
                principalColumn: "Plate",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
