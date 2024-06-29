using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartReservationCinema.Migrations
{
    public partial class addFavouriteFilm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrices_HallSectors_SectorId",
                table: "TicketPrices");

            migrationBuilder.AlterColumn<int>(
                name: "SectorId",
                table: "TicketPrices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "TicketPrices",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
           
            migrationBuilder.CreateTable(
                name: "FavouriteFilms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FilmId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteFilms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavouriteFilms_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouriteFilms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteFilms_FilmId",
                table: "FavouriteFilms",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteFilms_UserId",
                table: "FavouriteFilms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrices_HallSectors_SectorId",
                table: "TicketPrices",
                column: "SectorId",
                principalTable: "HallSectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrices_HallSectors_SectorId",
                table: "TicketPrices");

            migrationBuilder.DropTable(
                name: "FavouriteFilms");

            migrationBuilder.AlterColumn<int>(
                name: "SectorId",
                table: "TicketPrices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "TicketPrices",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrices_HallSectors_SectorId",
                table: "TicketPrices",
                column: "SectorId",
                principalTable: "HallSectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
