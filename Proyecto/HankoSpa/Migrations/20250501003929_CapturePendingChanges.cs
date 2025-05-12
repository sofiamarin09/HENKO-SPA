using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HankoSpa.Migrations
{
    /// <inheritdoc />
    public partial class CapturePendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitasServicios_Citas_CitasID",
                table: "CitasServicios");

            migrationBuilder.RenameColumn(
                name: "CitasID",
                table: "CitasServicios",
                newName: "CitaId");

            migrationBuilder.RenameIndex(
                name: "IX_CitasServicios_CitasID",
                table: "CitasServicios",
                newName: "IX_CitasServicios_CitaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CitasServicios_Citas_CitaId",
                table: "CitasServicios",
                column: "CitaId",
                principalTable: "Citas",
                principalColumn: "CitaId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitasServicios_Citas_CitaId",
                table: "CitasServicios");

            migrationBuilder.RenameColumn(
                name: "CitaId",
                table: "CitasServicios",
                newName: "CitasID");

            migrationBuilder.RenameIndex(
                name: "IX_CitasServicios_CitaId",
                table: "CitasServicios",
                newName: "IX_CitasServicios_CitasID");

            migrationBuilder.AddForeignKey(
                name: "FK_CitasServicios_Citas_CitasID",
                table: "CitasServicios",
                column: "CitasID",
                principalTable: "Citas",
                principalColumn: "CitaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
