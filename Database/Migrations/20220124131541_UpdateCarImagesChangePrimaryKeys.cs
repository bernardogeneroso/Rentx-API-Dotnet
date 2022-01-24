using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class UpdateCarImagesChangePrimaryKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CarsImages",
                table: "CarsImages");

            migrationBuilder.DropIndex(
                name: "IX_CarsImages_Plate",
                table: "CarsImages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CarsImages");

            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "CarsImages",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageName",
                table: "CarsImages",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarsImages",
                table: "CarsImages",
                columns: new[] { "Plate", "ImageName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CarsImages",
                table: "CarsImages");

            migrationBuilder.AlterColumn<string>(
                name: "ImageName",
                table: "CarsImages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "CarsImages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CarsImages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarsImages",
                table: "CarsImages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CarsImages_Plate",
                table: "CarsImages",
                column: "Plate");
        }
    }
}
