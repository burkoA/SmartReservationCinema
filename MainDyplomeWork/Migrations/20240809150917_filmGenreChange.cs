using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartReservationCinema.Migrations
{
    public partial class filmGenreChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenresFilmes_Films_Id_Film",
                table: "GenresFilmes");

            migrationBuilder.DropForeignKey(
                name: "FK_GenresFilmes_Genres_Id_Genre",
                table: "GenresFilmes");

            migrationBuilder.RenameColumn(
                name: "Id_Genre",
                table: "GenresFilmes",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "Id_Film",
                table: "GenresFilmes",
                newName: "FilmId");

            migrationBuilder.RenameColumn(
                name: "Id_Genre_Film",
                table: "GenresFilmes",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_GenresFilmes_Id_Genre",
                table: "GenresFilmes",
                newName: "IX_GenresFilmes_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_GenresFilmes_Id_Film",
                table: "GenresFilmes",
                newName: "IX_GenresFilmes_FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_GenresFilmes_Films_FilmId",
                table: "GenresFilmes",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenresFilmes_Genres_GenreId",
                table: "GenresFilmes",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenresFilmes_Films_FilmId",
                table: "GenresFilmes");

            migrationBuilder.DropForeignKey(
                name: "FK_GenresFilmes_Genres_GenreId",
                table: "GenresFilmes");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "GenresFilmes",
                newName: "Id_Genre");

            migrationBuilder.RenameColumn(
                name: "FilmId",
                table: "GenresFilmes",
                newName: "Id_Film");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GenresFilmes",
                newName: "Id_Genre_Film");

            migrationBuilder.RenameIndex(
                name: "IX_GenresFilmes_GenreId",
                table: "GenresFilmes",
                newName: "IX_GenresFilmes_Id_Genre");

            migrationBuilder.RenameIndex(
                name: "IX_GenresFilmes_FilmId",
                table: "GenresFilmes",
                newName: "IX_GenresFilmes_Id_Film");

            migrationBuilder.AddForeignKey(
                name: "FK_GenresFilmes_Films_Id_Film",
                table: "GenresFilmes",
                column: "Id_Film",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenresFilmes_Genres_Id_Genre",
                table: "GenresFilmes",
                column: "Id_Genre",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
