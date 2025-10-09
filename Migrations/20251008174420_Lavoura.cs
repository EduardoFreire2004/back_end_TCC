using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_TCC.Migrations
{
    /// <inheritdoc />
    public partial class Lavoura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Lavouras");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Lavouras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "latitude",
                table: "Lavouras",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "longitude",
                table: "Lavouras",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
