using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_TCC.Migrations
{
    /// <inheritdoc />
    public partial class Insumo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insumos_Categoria_Insumo_categoriaID",
                table: "Insumos");

            migrationBuilder.DropForeignKey(
                name: "FK_Insumos_Fornecedores_Agrotoxicos_fornecedorID",
                table: "Insumos");

            migrationBuilder.DropIndex(
                name: "IX_Insumos_categoriaID",
                table: "Insumos");

            migrationBuilder.DropIndex(
                name: "IX_Insumos_fornecedorID",
                table: "Insumos");

            migrationBuilder.AlterColumn<string>(
                name: "unidade_Medida",
                table: "Insumos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "unidade_Medida",
                table: "Insumos",
                type: "real",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_categoriaID",
                table: "Insumos",
                column: "categoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_fornecedorID",
                table: "Insumos",
                column: "fornecedorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Insumos_Categoria_Insumo_categoriaID",
                table: "Insumos",
                column: "categoriaID",
                principalTable: "Categoria_Insumo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Insumos_Fornecedores_Agrotoxicos_fornecedorID",
                table: "Insumos",
                column: "fornecedorID",
                principalTable: "Fornecedores_Agrotoxicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
