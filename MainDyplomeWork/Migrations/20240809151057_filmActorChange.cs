using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartReservationCinema.Migrations
{
    public partial class filmActorChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmActor_Actors_Id_Actor",
                table: "FilmActor");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmActor_Films_Id_Film",
                table: "FilmActor");

            migrationBuilder.RenameColumn(
                name: "Id_Film",
                table: "FilmActor",
                newName: "FilmId");

            migrationBuilder.RenameColumn(
                name: "Id_Actor",
                table: "FilmActor",
                newName: "ActorId");

            migrationBuilder.RenameColumn(
                name: "Id_Film_Actor",
                table: "FilmActor",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_FilmActor_Id_Film",
                table: "FilmActor",
                newName: "IX_FilmActor_FilmId");

            migrationBuilder.RenameIndex(
                name: "IX_FilmActor_Id_Actor",
                table: "FilmActor",
                newName: "IX_FilmActor_ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmActor_Actors_ActorId",
                table: "FilmActor",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmActor_Films_FilmId",
                table: "FilmActor",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmActor_Actors_ActorId",
                table: "FilmActor");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmActor_Films_FilmId",
                table: "FilmActor");

            migrationBuilder.RenameColumn(
                name: "FilmId",
                table: "FilmActor",
                newName: "Id_Film");

            migrationBuilder.RenameColumn(
                name: "ActorId",
                table: "FilmActor",
                newName: "Id_Actor");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FilmActor",
                newName: "Id_Film_Actor");

            migrationBuilder.RenameIndex(
                name: "IX_FilmActor_FilmId",
                table: "FilmActor",
                newName: "IX_FilmActor_Id_Film");

            migrationBuilder.RenameIndex(
                name: "IX_FilmActor_ActorId",
                table: "FilmActor",
                newName: "IX_FilmActor_Id_Actor");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmActor_Actors_Id_Actor",
                table: "FilmActor",
                column: "Id_Actor",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmActor_Films_Id_Film",
                table: "FilmActor",
                column: "Id_Film",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
