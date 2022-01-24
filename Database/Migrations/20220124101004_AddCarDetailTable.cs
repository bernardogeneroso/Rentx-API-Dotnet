using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class AddCarDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarDetail",
                columns: table => new
                {
                    Plate = table.Column<string>(type: "TEXT", nullable: false),
                    maxSpeed = table.Column<int>(type: "INTEGER", nullable: false),
                    topSpeed = table.Column<int>(type: "INTEGER", nullable: false),
                    acceleration = table.Column<float>(type: "REAL", nullable: false),
                    weight = table.Column<int>(type: "INTEGER", nullable: false),
                    hp = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarDetail", x => x.Plate);
                    table.ForeignKey(
                        name: "FK_CarDetail_Cars_Plate",
                        column: x => x.Plate,
                        principalTable: "Cars",
                        principalColumn: "Plate",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarDetail");
        }
    }
}
