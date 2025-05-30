using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_TCC.Migrations
{
    /// <inheritdoc />
    public partial class Lavoura1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lavouras_Aplicacoes_aplicacaoID",
                table: "Lavouras");

            migrationBuilder.DropForeignKey(
                name: "FK_Lavouras_Colheitas_colheitaID",
                table: "Lavouras");

            migrationBuilder.DropForeignKey(
                name: "FK_Lavouras_Insumos_insumoID",
                table: "Lavouras");

            migrationBuilder.DropForeignKey(
                name: "FK_Lavouras_Plantios_plantioID",
                table: "Lavouras");

            migrationBuilder.DropIndex(
                name: "IX_Lavouras_aplicacaoID",
                table: "Lavouras");

            migrationBuilder.DropIndex(
                name: "IX_Lavouras_colheitaID",
                table: "Lavouras");

            migrationBuilder.DropIndex(
                name: "IX_Lavouras_insumoID",
                table: "Lavouras");

            migrationBuilder.DropIndex(
                name: "IX_Lavouras_plantioID",
                table: "Lavouras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Lavouras_aplicacaoID",
                table: "Lavouras",
                column: "aplicacaoID");

            migrationBuilder.CreateIndex(
                name: "IX_Lavouras_colheitaID",
                table: "Lavouras",
                column: "colheitaID");

            migrationBuilder.CreateIndex(
                name: "IX_Lavouras_insumoID",
                table: "Lavouras",
                column: "insumoID");

            migrationBuilder.CreateIndex(
                name: "IX_Lavouras_plantioID",
                table: "Lavouras",
                column: "plantioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lavouras_Aplicacoes_aplicacaoID",
                table: "Lavouras",
                column: "aplicacaoID",
                principalTable: "Aplicacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lavouras_Colheitas_colheitaID",
                table: "Lavouras",
                column: "colheitaID",
                principalTable: "Colheitas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lavouras_Insumos_insumoID",
                table: "Lavouras",
                column: "insumoID",
                principalTable: "Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lavouras_Plantios_plantioID",
                table: "Lavouras",
                column: "plantioID",
                principalTable: "Plantios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
