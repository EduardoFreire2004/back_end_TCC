using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_TCC.Migrations
{
    /// <inheritdoc />
    public partial class Inicial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plantios_Sementes_sementeID",
                table: "Plantios");

            migrationBuilder.DropForeignKey(
                name: "FK_Sementes_Fornecedores_Sementes_fornecedorID",
                table: "Sementes");

            migrationBuilder.DropIndex(
                name: "IX_Sementes_fornecedorID",
                table: "Sementes");

            migrationBuilder.DropIndex(
                name: "IX_Plantios_sementeID",
                table: "Plantios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sementes_fornecedorID",
                table: "Sementes",
                column: "fornecedorID");

            migrationBuilder.CreateIndex(
                name: "IX_Plantios_sementeID",
                table: "Plantios",
                column: "sementeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Plantios_Sementes_sementeID",
                table: "Plantios",
                column: "sementeID",
                principalTable: "Sementes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sementes_Fornecedores_Sementes_fornecedorID",
                table: "Sementes",
                column: "fornecedorID",
                principalTable: "Fornecedores_Sementes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
