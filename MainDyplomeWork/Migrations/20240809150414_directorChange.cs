using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartReservationCinema.Migrations
{
    public partial class directorChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Work_Experience",
                table: "Director",
                newName: "WorkExperience");

            migrationBuilder.RenameColumn(
                name: "Name_Director",
                table: "Director",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Movie_Number",
                table: "Director",
                newName: "MovieNumber");

            migrationBuilder.RenameColumn(
                name: "Birth_Place",
                table: "Director",
                newName: "BirthPlace");

            migrationBuilder.RenameColumn(
                name: "Id_Director",
                table: "Director",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkExperience",
                table: "Director",
                newName: "Work_Experience");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Director",
                newName: "Name_Director");

            migrationBuilder.RenameColumn(
                name: "MovieNumber",
                table: "Director",
                newName: "Movie_Number");

            migrationBuilder.RenameColumn(
                name: "BirthPlace",
                table: "Director",
                newName: "Birth_Place");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Director",
                newName: "Id_Director");
        }
    }
}
