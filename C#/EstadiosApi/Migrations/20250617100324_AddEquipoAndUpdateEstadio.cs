using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EstadiosApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipoAndUpdateEstadio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Estadios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Aforo",
                table: "Estadios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EquipoId",
                table: "Estadios",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInauguracion",
                table: "Estadios",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AñoFundacion = table.Column<int>(type: "integer", nullable: false),
                    Colores = table.Column<string>(type: "text", nullable: false),
                    Pais = table.Column<string>(type: "text", nullable: false),
                    Ciudad = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estadios_EquipoId",
                table: "Estadios",
                column: "EquipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estadios_Equipos_EquipoId",
                table: "Estadios",
                column: "EquipoId",
                principalTable: "Equipos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estadios_Equipos_EquipoId",
                table: "Estadios");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropIndex(
                name: "IX_Estadios_EquipoId",
                table: "Estadios");

            migrationBuilder.DropColumn(
                name: "Aforo",
                table: "Estadios");

            migrationBuilder.DropColumn(
                name: "EquipoId",
                table: "Estadios");

            migrationBuilder.DropColumn(
                name: "FechaInauguracion",
                table: "Estadios");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Estadios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
