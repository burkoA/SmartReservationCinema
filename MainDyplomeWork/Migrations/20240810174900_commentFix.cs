using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartReservationCinema.Migrations
{
    public partial class commentFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Films_IdFilm",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_IdUser",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_IdFilm",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_IdUser",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IdFilm",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FilmId",
                table: "Comments",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Films_FilmId",
                table: "Comments",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Films_FilmId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_FilmId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "IdFilm",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUser",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdFilm",
                table: "Comments",
                column: "IdFilm");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdUser",
                table: "Comments",
                column: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Films_IdFilm",
                table: "Comments",
                column: "IdFilm",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_IdUser",
                table: "Comments",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
