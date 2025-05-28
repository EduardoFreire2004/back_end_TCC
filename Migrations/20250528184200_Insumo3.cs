using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_TCC.Migrations
{
    /// <inheritdoc />
    public partial class Insumo3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fornecedorID",
                table: "Insumos",
                newName: "fornecedorInsumoID");

            migrationBuilder.RenameColumn(
                name: "categoriaID",
                table: "Insumos",
                newName: "categoriaInsumoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fornecedorInsumoID",
                table: "Insumos",
                newName: "fornecedorID");

            migrationBuilder.RenameColumn(
                name: "categoriaInsumoID",
                table: "Insumos",
                newName: "categoriaID");
        }
    }
}
