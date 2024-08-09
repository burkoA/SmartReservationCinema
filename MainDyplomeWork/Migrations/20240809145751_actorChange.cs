using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartReservationCinema.Migrations
{
    public partial class actorChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Actor_Name",
                table: "Actors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Id_Actor",
                table: "Actors",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "SectorName",
                table: "HallSectors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HallName",
                table: "Halls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Actors",
                newName: "Actor_Name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Actors",
                newName: "Id_Actor");

            migrationBuilder.AlterColumn<string>(
                name: "SectorName",
                table: "HallSectors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "HallName",
                table: "Halls",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
