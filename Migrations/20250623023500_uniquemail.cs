using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticaLogin.Migrations
{
    /// <inheritdoc />
    public partial class uniquemail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Correo",
                table: "Usuario",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuario_Correo",
                table: "Usuario");
        }
    }
}
