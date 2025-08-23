using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_TCC.Migrations
{
    /// <inheritdoc />
    public partial class inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria_Insumo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria_Insumo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categoria_Insumo_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cnpj = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fornecedores_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lavouras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    area = table.Column<float>(type: "real", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    latitude = table.Column<float>(type: "real", nullable: false),
                    longitude = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lavouras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lavouras_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataExpiracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Agrotoxicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Agrotoxicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tipo_Agrotoxicos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Insumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    categoriaInsumoID = table.Column<int>(type: "int", nullable: false),
                    fornecedorID = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    unidade_Medida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    data_Cadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false),
                    preco = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insumos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insumos_Categoria_Insumo_categoriaInsumoID",
                        column: x => x.categoriaInsumoID,
                        principalTable: "Categoria_Insumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Insumos_Fornecedores_fornecedorID",
                        column: x => x.fornecedorID,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Insumos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sementes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    fornecedorID = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false),
                    data_Cadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    preco = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sementes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sementes_Fornecedores_fornecedorID",
                        column: x => x.fornecedorID,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sementes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Colheitas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    lavouraID = table.Column<int>(type: "int", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    quantidadeSacas = table.Column<double>(type: "float", nullable: false),
                    areaHectares = table.Column<double>(type: "float", nullable: false),
                    cooperativaDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precoPorSaca = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colheitas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Colheitas_Lavouras_lavouraID",
                        column: x => x.lavouraID,
                        principalTable: "Lavouras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Colheitas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agrotoxicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    fornecedorID = table.Column<int>(type: "int", nullable: false),
                    tipoID = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    unidade_Medida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    data_Cadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false),
                    preco = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agrotoxicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agrotoxicos_Fornecedores_fornecedorID",
                        column: x => x.fornecedorID,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agrotoxicos_Tipo_Agrotoxicos_tipoID",
                        column: x => x.tipoID,
                        principalTable: "Tipo_Agrotoxicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agrotoxicos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aplicacao_Insumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    lavouraID = table.Column<int>(type: "int", nullable: false),
                    insumoID = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aplicacao_Insumos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aplicacao_Insumos_Insumos_insumoID",
                        column: x => x.insumoID,
                        principalTable: "Insumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aplicacao_Insumos_Lavouras_lavouraID",
                        column: x => x.lavouraID,
                        principalTable: "Lavouras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aplicacao_Insumos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plantios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    lavouraID = table.Column<int>(type: "int", nullable: false),
                    sementeID = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    areaPlantada = table.Column<float>(type: "real", nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plantios_Lavouras_lavouraID",
                        column: x => x.lavouraID,
                        principalTable: "Lavouras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plantios_Sementes_sementeID",
                        column: x => x.sementeID,
                        principalTable: "Sementes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plantios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aplicacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    lavouraID = table.Column<int>(type: "int", nullable: false),
                    agrotoxicoID = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    qtde = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aplicacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aplicacoes_Agrotoxicos_agrotoxicoID",
                        column: x => x.agrotoxicoID,
                        principalTable: "Agrotoxicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aplicacoes_Lavouras_lavouraID",
                        column: x => x.lavouraID,
                        principalTable: "Lavouras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aplicacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacoesEstoque",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    lavouraID = table.Column<int>(type: "int", nullable: false),
                    movimentacao = table.Column<int>(type: "int", nullable: false),
                    agrotoxicoID = table.Column<int>(type: "int", nullable: true),
                    sementeID = table.Column<int>(type: "int", nullable: true),
                    insumoID = table.Column<int>(type: "int", nullable: true),
                    qtde = table.Column<float>(type: "real", nullable: false),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    origemAplicacaoID = table.Column<int>(type: "int", nullable: true),
                    origemAplicacaoInsumoID = table.Column<int>(type: "int", nullable: true),
                    origemPlantioID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacoesEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Agrotoxicos_agrotoxicoID",
                        column: x => x.agrotoxicoID,
                        principalTable: "Agrotoxicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Insumos_insumoID",
                        column: x => x.insumoID,
                        principalTable: "Insumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Lavouras_lavouraID",
                        column: x => x.lavouraID,
                        principalTable: "Lavouras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Sementes_sementeID",
                        column: x => x.sementeID,
                        principalTable: "Sementes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Custos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    lavouraID = table.Column<int>(type: "int", nullable: false),
                    aplicacaoAgrotoxicoID = table.Column<int>(type: "int", nullable: true),
                    aplicacaoInsumoID = table.Column<int>(type: "int", nullable: true),
                    plantioID = table.Column<int>(type: "int", nullable: true),
                    colheitaID = table.Column<int>(type: "int", nullable: true),
                    custoTotal = table.Column<double>(type: "float", nullable: false),
                    ganhoTotal = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Custos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Custos_Aplicacao_Insumos_aplicacaoInsumoID",
                        column: x => x.aplicacaoInsumoID,
                        principalTable: "Aplicacao_Insumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Custos_Aplicacoes_aplicacaoAgrotoxicoID",
                        column: x => x.aplicacaoAgrotoxicoID,
                        principalTable: "Aplicacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Custos_Colheitas_colheitaID",
                        column: x => x.colheitaID,
                        principalTable: "Colheitas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Custos_Lavouras_lavouraID",
                        column: x => x.lavouraID,
                        principalTable: "Lavouras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Custos_Plantios_plantioID",
                        column: x => x.plantioID,
                        principalTable: "Plantios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Custos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
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
                name: "IX_Agrotoxicos_UsuarioId",
                table: "Agrotoxicos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacao_Insumos_insumoID",
                table: "Aplicacao_Insumos",
                column: "insumoID");

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacao_Insumos_lavouraID",
                table: "Aplicacao_Insumos",
                column: "lavouraID");

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacao_Insumos_UsuarioId",
                table: "Aplicacao_Insumos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacoes_agrotoxicoID",
                table: "Aplicacoes",
                column: "agrotoxicoID");

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacoes_lavouraID",
                table: "Aplicacoes",
                column: "lavouraID");

            migrationBuilder.CreateIndex(
                name: "IX_Aplicacoes_UsuarioId",
                table: "Aplicacoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Insumo_UsuarioId",
                table: "Categoria_Insumo",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Colheitas_lavouraID",
                table: "Colheitas",
                column: "lavouraID");

            migrationBuilder.CreateIndex(
                name: "IX_Colheitas_UsuarioId",
                table: "Colheitas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Custos_aplicacaoAgrotoxicoID",
                table: "Custos",
                column: "aplicacaoAgrotoxicoID");

            migrationBuilder.CreateIndex(
                name: "IX_Custos_aplicacaoInsumoID",
                table: "Custos",
                column: "aplicacaoInsumoID");

            migrationBuilder.CreateIndex(
                name: "IX_Custos_colheitaID",
                table: "Custos",
                column: "colheitaID");

            migrationBuilder.CreateIndex(
                name: "IX_Custos_lavouraID",
                table: "Custos",
                column: "lavouraID");

            migrationBuilder.CreateIndex(
                name: "IX_Custos_plantioID",
                table: "Custos",
                column: "plantioID");

            migrationBuilder.CreateIndex(
                name: "IX_Custos_UsuarioId",
                table: "Custos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_UsuarioId",
                table: "Fornecedores",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_categoriaInsumoID",
                table: "Insumos",
                column: "categoriaInsumoID");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_fornecedorID",
                table: "Insumos",
                column: "fornecedorID");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_UsuarioId",
                table: "Insumos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Lavouras_UsuarioId",
                table: "Lavouras",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_agrotoxicoID",
                table: "MovimentacoesEstoque",
                column: "agrotoxicoID");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_insumoID",
                table: "MovimentacoesEstoque",
                column: "insumoID");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_lavouraID",
                table: "MovimentacoesEstoque",
                column: "lavouraID");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_sementeID",
                table: "MovimentacoesEstoque",
                column: "sementeID");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_UsuarioId",
                table: "MovimentacoesEstoque",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Plantios_lavouraID",
                table: "Plantios",
                column: "lavouraID");

            migrationBuilder.CreateIndex(
                name: "IX_Plantios_sementeID",
                table: "Plantios",
                column: "sementeID");

            migrationBuilder.CreateIndex(
                name: "IX_Plantios_UsuarioId",
                table: "Plantios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UsuarioId",
                table: "RefreshTokens",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Sementes_fornecedorID",
                table: "Sementes",
                column: "fornecedorID");

            migrationBuilder.CreateIndex(
                name: "IX_Sementes_UsuarioId",
                table: "Sementes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Tipo_Agrotoxicos_UsuarioId",
                table: "Tipo_Agrotoxicos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Custos");

            migrationBuilder.DropTable(
                name: "MovimentacoesEstoque");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Aplicacao_Insumos");

            migrationBuilder.DropTable(
                name: "Aplicacoes");

            migrationBuilder.DropTable(
                name: "Colheitas");

            migrationBuilder.DropTable(
                name: "Plantios");

            migrationBuilder.DropTable(
                name: "Insumos");

            migrationBuilder.DropTable(
                name: "Agrotoxicos");

            migrationBuilder.DropTable(
                name: "Lavouras");

            migrationBuilder.DropTable(
                name: "Sementes");

            migrationBuilder.DropTable(
                name: "Categoria_Insumo");

            migrationBuilder.DropTable(
                name: "Tipo_Agrotoxicos");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
