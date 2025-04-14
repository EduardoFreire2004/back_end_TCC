using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_TCC.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria_Insumo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria_Insumo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colheitas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colheitas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores_Agrotoxicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cnpj = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores_Agrotoxicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores_Insumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cnpj = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores_Insumos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores_Sementes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cnpj = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores_Sementes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Agrotoxicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Agrotoxicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Movimentacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Movimentacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Insumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoriaID = table.Column<int>(type: "int", nullable: false),
                    fornecedorID = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    unidade_Medida = table.Column<float>(type: "real", nullable: false),
                    data_Cadastro = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insumos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insumos_Categoria_Insumo_categoriaID",
                        column: x => x.categoriaID,
                        principalTable: "Categoria_Insumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Insumos_Fornecedores_Agrotoxicos_fornecedorID",
                        column: x => x.fornecedorID,
                        principalTable: "Fornecedores_Agrotoxicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sementes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fornecedorID = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sementes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sementes_Fornecedores_Sementes_fornecedorID",
                        column: x => x.fornecedorID,
                        principalTable: "Fornecedores_Sementes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agrotoxicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fornecedorID = table.Column<int>(type: "int", nullable: false),
                    tipoID = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    unidade_Medida = table.Column<float>(type: "real", nullable: false),
                    data_Cadastro = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agrotoxicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agrotoxicos_Fornecedores_Agrotoxicos_fornecedorID",
                        column: x => x.fornecedorID,
                        principalTable: "Fornecedores_Agrotoxicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agrotoxicos_Tipo_Agrotoxicos_tipoID",
                        column: x => x.tipoID,
                        principalTable: "Tipo_Agrotoxicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plantios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sementeID = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    areaPlantada = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plantios_Sementes_sementeID",
                        column: x => x.sementeID,
                        principalTable: "Sementes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aplicacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    agrotoxicoID = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aplicacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aplicacoes_Agrotoxicos_agrotoxicoID",
                        column: x => x.agrotoxicoID,
                        principalTable: "Agrotoxicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lavouras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    insumoID = table.Column<int>(type: "int", nullable: false),
                    aplicacaoID = table.Column<int>(type: "int", nullable: false),
                    plantioID = table.Column<int>(type: "int", nullable: false),
                    colheitaID = table.Column<int>(type: "int", nullable: false),
                    area = table.Column<float>(type: "real", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lavouras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lavouras_Aplicacoes_aplicacaoID",
                        column: x => x.aplicacaoID,
                        principalTable: "Aplicacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lavouras_Colheitas_colheitaID",
                        column: x => x.colheitaID,
                        principalTable: "Colheitas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lavouras_Insumos_insumoID",
                        column: x => x.insumoID,
                        principalTable: "Insumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lavouras_Plantios_plantioID",
                        column: x => x.plantioID,
                        principalTable: "Plantios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Movimentacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lavouraID = table.Column<int>(type: "int", nullable: false),
                    movimentacaoId = table.Column<int>(type: "int", nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_Lavouras_lavouraID",
                        column: x => x.lavouraID,
                        principalTable: "Lavouras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_Tipo_Movimentacoes_movimentacaoId",
                        column: x => x.movimentacaoId,
                        principalTable: "Tipo_Movimentacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agrotoxicos_fornecedorID",
                table: "Agrotoxicos",
                column: "fornecedorID");

            migrationBuilder.CreateIndex(
                name: "IX_Agrotoxicos_tipoID",
                table: "Agrotoxicos",
                column: "tipoID");

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacoes_agrotoxicoID",
                table: "Aplicacoes",
                column: "agrotoxicoID");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_categoriaID",
                table: "Insumos",
                column: "categoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_fornecedorID",
                table: "Insumos",
                column: "fornecedorID");

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

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_lavouraID",
                table: "Movimentacoes",
                column: "lavouraID");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_movimentacaoId",
                table: "Movimentacoes",
                column: "movimentacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Plantios_sementeID",
                table: "Plantios",
                column: "sementeID");

            migrationBuilder.CreateIndex(
                name: "IX_Sementes_fornecedorID",
                table: "Sementes",
                column: "fornecedorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fornecedores_Insumos");

            migrationBuilder.DropTable(
                name: "Movimentacoes");

            migrationBuilder.DropTable(
                name: "Lavouras");

            migrationBuilder.DropTable(
                name: "Tipo_Movimentacoes");

            migrationBuilder.DropTable(
                name: "Aplicacoes");

            migrationBuilder.DropTable(
                name: "Colheitas");

            migrationBuilder.DropTable(
                name: "Insumos");

            migrationBuilder.DropTable(
                name: "Plantios");

            migrationBuilder.DropTable(
                name: "Agrotoxicos");

            migrationBuilder.DropTable(
                name: "Categoria_Insumo");

            migrationBuilder.DropTable(
                name: "Sementes");

            migrationBuilder.DropTable(
                name: "Fornecedores_Agrotoxicos");

            migrationBuilder.DropTable(
                name: "Tipo_Agrotoxicos");

            migrationBuilder.DropTable(
                name: "Fornecedores_Sementes");
        }
    }
}
