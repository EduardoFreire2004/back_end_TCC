using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_TCC.Migrations
{
    /// <inheritdoc />
    public partial class LavouraNoActionDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Aplicacao_Insumos_aplicacaoInsumoID",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Aplicacoes_aplicacaoAgrotoxicoID",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Colheitas_colheitaID",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Lavouras_lavouraID",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Plantios_plantioID",
                table: "Custos");

            migrationBuilder.RenameColumn(
                name: "lavouraID",
                table: "Custos",
                newName: "LavouraId");

            migrationBuilder.RenameIndex(
                name: "IX_Custos_lavouraID",
                table: "Custos",
                newName: "IX_Custos_LavouraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Aplicacao_Insumos_aplicacaoInsumoID",
                table: "Custos",
                column: "aplicacaoInsumoID",
                principalTable: "Aplicacao_Insumos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Aplicacoes_aplicacaoAgrotoxicoID",
                table: "Custos",
                column: "aplicacaoAgrotoxicoID",
                principalTable: "Aplicacoes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Colheitas_colheitaID",
                table: "Custos",
                column: "colheitaID",
                principalTable: "Colheitas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Lavouras_LavouraId",
                table: "Custos",
                column: "LavouraId",
                principalTable: "Lavouras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Plantios_plantioID",
                table: "Custos",
                column: "plantioID",
                principalTable: "Plantios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Aplicacao_Insumos_aplicacaoInsumoID",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Aplicacoes_aplicacaoAgrotoxicoID",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Colheitas_colheitaID",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Lavouras_LavouraId",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Plantios_plantioID",
                table: "Custos");

            migrationBuilder.RenameColumn(
                name: "LavouraId",
                table: "Custos",
                newName: "lavouraID");

            migrationBuilder.RenameIndex(
                name: "IX_Custos_LavouraId",
                table: "Custos",
                newName: "IX_Custos_lavouraID");

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Aplicacao_Insumos_aplicacaoInsumoID",
                table: "Custos",
                column: "aplicacaoInsumoID",
                principalTable: "Aplicacao_Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Aplicacoes_aplicacaoAgrotoxicoID",
                table: "Custos",
                column: "aplicacaoAgrotoxicoID",
                principalTable: "Aplicacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Colheitas_colheitaID",
                table: "Custos",
                column: "colheitaID",
                principalTable: "Colheitas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Lavouras_lavouraID",
                table: "Custos",
                column: "lavouraID",
                principalTable: "Lavouras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Plantios_plantioID",
                table: "Custos",
                column: "plantioID",
                principalTable: "Plantios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
