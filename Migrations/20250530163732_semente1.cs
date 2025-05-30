using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_TCC.Migrations
{
    /// <inheritdoc />
    public partial class semente1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fornecedorID",
                table: "Sementes",
                newName: "fornecedorSementeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fornecedorSementeID",
                table: "Sementes",
                newName: "fornecedorID");
        }
    }
}
