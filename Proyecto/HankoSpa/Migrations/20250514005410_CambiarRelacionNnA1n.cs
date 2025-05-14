using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HankoSpa.Migrations
{
    /// <inheritdoc />
    public partial class CambiarRelacionNnA1n : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CitasServicios");

            migrationBuilder.AddColumn<int>(
                name: "ServicioID",
                table: "Citas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Citas_ServicioID",
                table: "Citas",
                column: "ServicioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Servicios_ServicioID",
                table: "Citas",
                column: "ServicioID",
                principalTable: "Servicios",
                principalColumn: "ServicioId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Servicios_ServicioID",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_ServicioID",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "ServicioID",
                table: "Citas");

            migrationBuilder.CreateTable(
                name: "CitasServicios",
                columns: table => new
                {
                    Citas_ServiciosID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitaId = table.Column<int>(type: "int", nullable: false),
                    ServicioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitasServicios", x => x.Citas_ServiciosID);
                    table.ForeignKey(
                        name: "FK_CitasServicios_Citas_CitaId",
                        column: x => x.CitaId,
                        principalTable: "Citas",
                        principalColumn: "CitaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CitasServicios_Servicios_ServicioID",
                        column: x => x.ServicioID,
                        principalTable: "Servicios",
                        principalColumn: "ServicioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CitasServicios_CitaId",
                table: "CitasServicios",
                column: "CitaId");

            migrationBuilder.CreateIndex(
                name: "IX_CitasServicios_ServicioID",
                table: "CitasServicios",
                column: "ServicioID");
        }
    }
}
