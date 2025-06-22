using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstadiosApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordinatesAndPhotoToEstadio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Estadios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Estadios",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Estadios",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Estadios");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Estadios");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Estadios");
        }
    }
}
