using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class AdjustOnTableCarAppointments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "CarsAppointments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "CarsAppointments",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "rentalPrice",
                table: "CarsAppointments",
                newName: "RentalPrice");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "CarsAppointments",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "CarsAppointments",
                newName: "CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "CarsAppointments",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "CarsAppointments",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "RentalPrice",
                table: "CarsAppointments",
                newName: "rentalPrice");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "CarsAppointments",
                newName: "endDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CarsAppointments",
                newName: "createdAt");
        }
    }
}
