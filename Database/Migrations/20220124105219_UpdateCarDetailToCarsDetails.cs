using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class UpdateCarDetailToCarsDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDetail_Cars_Plate",
                table: "CarDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarDetail",
                table: "CarDetail");

            migrationBuilder.RenameTable(
                name: "CarDetail",
                newName: "CarsDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarsDetails",
                table: "CarsDetails",
                column: "Plate");

            migrationBuilder.AddForeignKey(
                name: "FK_CarsDetails_Cars_Plate",
                table: "CarsDetails",
                column: "Plate",
                principalTable: "Cars",
                principalColumn: "Plate",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarsDetails_Cars_Plate",
                table: "CarsDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarsDetails",
                table: "CarsDetails");

            migrationBuilder.RenameTable(
                name: "CarsDetails",
                newName: "CarDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarDetail",
                table: "CarDetail",
                column: "Plate");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDetail_Cars_Plate",
                table: "CarDetail",
                column: "Plate",
                principalTable: "Cars",
                principalColumn: "Plate",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
