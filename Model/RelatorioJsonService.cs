using API_TCC.DTOs;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Data.SqlClient;

namespace API_TCC.Model
{
    public class RelatorioJsonService : IRelatorioJsonService
    {
        private readonly Contexto _contexto;

        public RelatorioJsonService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<RelatorioFornecedoresResponseDto> GerarRelatorioFornecedoresAsync(int usuarioId, int lavouraId)
        {
            try
            {
                using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
                
                var sql = @"
                    SELECT 
                        f.Id,
                        f.nome as Nome,
                        f.cnpj as Cnpj,
                        f.telefone as Telefone,
                        (SELECT COUNT(*) FROM Agrotoxicos WHERE fornecedorID = f.Id AND UsuarioId = @UsuarioId) +
                        (SELECT COUNT(*) FROM Insumos WHERE fornecedorID = f.Id AND UsuarioId = @UsuarioId) +
                        (SELECT COUNT(*) FROM Sementes WHERE fornecedorID = f.Id AND UsuarioId = @UsuarioId) as TotalProdutos,
                        (SELECT ISNULL(SUM(qtde * preco), 0) FROM Agrotoxicos WHERE fornecedorID = f.Id AND UsuarioId = @UsuarioId) +
                        (SELECT ISNULL(SUM(qtde * preco), 0) FROM Insumos WHERE fornecedorID = f.Id AND UsuarioId = @UsuarioId) +
                        (SELECT ISNULL(SUM(qtde * preco), 0) FROM Sementes WHERE fornecedorID = f.Id AND UsuarioId = @UsuarioId) as ValorTotalProdutos
                    FROM Fornecedores f
                    WHERE f.UsuarioId = @UsuarioId
                    ORDER BY f.nome";

                var fornecedores = await connection.QueryAsync<RelatorioFornecedoresDto>(sql, new { UsuarioId = usuarioId });

                return new RelatorioFornecedoresResponseDto
                {
                    Success = true,
                    Data = fornecedores.ToList(),
                    TotalRegistros = fornecedores.Count()
                };
            }
            catch (Exception ex)
            {
                return new RelatorioFornecedoresResponseDto
                {
                    Success = false,
                    Error = $"Erro ao gerar relatório de fornecedores: {ex.Message}"
                };
            }
        }

        public async Task<RelatorioAplicacaoResponseDto> GerarRelatorioAplicacaoAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
                
                var sql = @"
                    SELECT 
                        a.Id,
                        ag.nome as Produto,
                        l.nome as Lavoura,
                        a.dataHora as DataAplicacao,
                        a.qtde as Quantidade,
                        ag.unidade_Medida as UnidadeMedida,
                        a.descricao as Observacoes
                    FROM Aplicacoes a
                    INNER JOIN Lavouras l ON a.lavouraID = l.Id
                    INNER JOIN Agrotoxicos ag ON a.agrotoxicoID = ag.Id
                    WHERE l.UsuarioId = @UsuarioId AND a.lavouraID = @LavouraId";

                object parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId };

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    sql += " AND a.dataHora BETWEEN @DataInicio AND @DataFim";
                    parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId, DataInicio = dataInicio.Value, DataFim = dataFim.Value };
                }

                sql += " ORDER BY a.dataHora DESC";

                var aplicacoes = await connection.QueryAsync<RelatorioAplicacaoDto>(sql, parameters);

                return new RelatorioAplicacaoResponseDto
                {
                    Success = true,
                    Data = aplicacoes.ToList(),
                    TotalRegistros = aplicacoes.Count()
                };
            }
            catch (Exception ex)
            {
                return new RelatorioAplicacaoResponseDto
                {
                    Success = false,
                    Error = $"Erro ao gerar relatório de aplicações: {ex.Message}"
                };
            }
        }

        public async Task<RelatorioAplicacaoInsumoResponseDto> GerarRelatorioAplicacaoInsumoAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
                
                var sql = @"
                    SELECT 
                        ai.Id,
                        i.nome as Insumo,
                        l.nome as Lavoura,
                        ai.dataHora as DataAplicacao,
                        ai.qtde as Quantidade,
                        ai.descricao as Descricao
                    FROM AplicacaoInsumos ai
                    INNER JOIN Insumos i ON ai.insumoID = i.Id
                    INNER JOIN Lavouras l ON ai.lavouraID = l.Id
                    WHERE l.UsuarioId = @UsuarioId AND ai.lavouraID = @LavouraId";

                object parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId };

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    sql += " AND ai.dataHora BETWEEN @DataInicio AND @DataFim";
                    parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId, DataInicio = dataInicio.Value, DataFim = dataFim.Value };
                }

                sql += " ORDER BY ai.dataHora DESC";

                var aplicacoes = await connection.QueryAsync<RelatorioAplicacaoInsumoDto>(sql, parameters);

                return new RelatorioAplicacaoInsumoResponseDto
                {
                    Success = true,
                    Data = aplicacoes.ToList(),
                    TotalRegistros = aplicacoes.Count()
                };
            }
            catch (Exception ex)
            {
                return new RelatorioAplicacaoInsumoResponseDto
                {
                    Success = false,
                    Error = $"Erro ao gerar relatório de aplicações de insumos: {ex.Message}"
                };
            }
        }

        public async Task<RelatorioSementeResponseDto> GerarRelatorioSementeAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
                
                var sql = @"
                    SELECT 
                        s.Id,
                        s.nome as Nome,
                        s.tipo as Tipo,
                        s.marca as Marca,
                        f.nome as Fornecedor,
                        s.qtde as Quantidade,
                        s.preco as Preco,
                        s.data_Cadastro as DataCadastro,
                        (s.qtde * s.preco) as ValorTotal
                    FROM Sementes s
                    INNER JOIN Fornecedores f ON s.fornecedorID = f.Id
                    WHERE s.UsuarioId = @UsuarioId";

                object parameters = new { UsuarioId = usuarioId };

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    sql += " AND s.data_Cadastro BETWEEN @DataInicio AND @DataFim";
                    parameters = new { UsuarioId = usuarioId, DataInicio = dataInicio.Value, DataFim = dataFim.Value };
                }

                sql += " ORDER BY s.data_Cadastro DESC";

                var sementes = await connection.QueryAsync<RelatorioSementeDto>(sql, parameters);

                return new RelatorioSementeResponseDto
                {
                    Success = true,
                    Data = sementes.ToList(),
                    TotalRegistros = sementes.Count()
                };
            }
            catch (Exception ex)
            {
                return new RelatorioSementeResponseDto
                {
                    Success = false,
                    Error = $"Erro ao gerar relatório de sementes: {ex.Message}"
                };
            }
        }

        public async Task<RelatorioInsumoResponseDto> GerarRelatorioInsumoAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
                
                var sql = @"
                    SELECT 
                        i.Id,
                        i.nome as Nome,
                        c.descricao as Categoria,
                        f.nome as Fornecedor,
                        i.qtde as Quantidade,
                        i.preco as Preco,
                        i.unidade_Medida as UnidadeMedida,
                        i.data_Cadastro as DataCadastro,
                        (i.qtde * i.preco) as ValorTotal
                    FROM Insumos i
                    INNER JOIN CategoriaInsumo c ON i.categoriaInsumoID = c.Id
                    INNER JOIN Fornecedores f ON i.fornecedorID = f.Id
                    WHERE i.UsuarioId = @UsuarioId";

                object parameters = new { UsuarioId = usuarioId };

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    sql += " AND i.data_Cadastro BETWEEN @DataInicio AND @DataFim";
                    parameters = new { UsuarioId = usuarioId, DataInicio = dataInicio.Value, DataFim = dataFim.Value };
                }

                sql += " ORDER BY i.data_Cadastro DESC";

                var insumos = await connection.QueryAsync<RelatorioInsumoDto>(sql, parameters);

                return new RelatorioInsumoResponseDto
                {
                    Success = true,
                    Data = insumos.ToList(),
                    TotalRegistros = insumos.Count()
                };
            }
            catch (Exception ex)
            {
                return new RelatorioInsumoResponseDto
                {
                    Success = false,
                    Error = $"Erro ao gerar relatório de insumos: {ex.Message}"
                };
            }
        }

        public async Task<RelatorioAgrotoxicoResponseDto> GerarRelatorioAgrotoxicoAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
                
                var sql = @"
                    SELECT 
                        a.Id,
                        a.nome as Nome,
                        t.descricao as Tipo,
                        f.nome as Fornecedor,
                        a.qtde as Quantidade,
                        a.preco as Preco,
                        a.unidade_Medida as UnidadeMedida,
                        a.data_Cadastro as DataCadastro,
                        (a.qtde * a.preco) as ValorTotal
                    FROM Agrotoxicos a
                    INNER JOIN TipoAgrotoxicos t ON a.tipoID = t.Id
                    INNER JOIN Fornecedores f ON a.fornecedorID = f.Id
                    WHERE a.UsuarioId = @UsuarioId";

                object parameters = new { UsuarioId = usuarioId };

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    sql += " AND a.data_Cadastro BETWEEN @DataInicio AND @DataFim";
                    parameters = new { UsuarioId = usuarioId, DataInicio = dataInicio.Value, DataFim = dataFim.Value };
                }

                sql += " ORDER BY a.data_Cadastro DESC";

                var agrotoxicos = await connection.QueryAsync<RelatorioAgrotoxicoDto>(sql, parameters);

                return new RelatorioAgrotoxicoResponseDto
                {
                    Success = true,
                    Data = agrotoxicos.ToList(),
                    TotalRegistros = agrotoxicos.Count()
                };
            }
            catch (Exception ex)
            {
                return new RelatorioAgrotoxicoResponseDto
                {
                    Success = false,
                    Error = $"Erro ao gerar relatório de agrotóxicos: {ex.Message}"
                };
            }
        }

        public async Task<RelatorioColheitaResponseDto> GerarRelatorioColheitaAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
                
                var sql = @"
                    SELECT 
                        c.Id,
                        c.tipo as Cultura,
                        l.nome as Lavoura,
                        c.dataHora as DataColheita,
                        c.quantidadeSacas as QuantidadeColhida,
                        (c.quantidadeSacas / c.areaHectares) as Produtividade,
                        c.descricao as Observacoes
                    FROM Colheitas c
                    INNER JOIN Lavouras l ON c.lavouraID = l.Id
                    WHERE l.UsuarioId = @UsuarioId AND c.lavouraID = @LavouraId";

                object parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId };

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    sql += " AND c.dataHora BETWEEN @DataInicio AND @DataFim";
                    parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId, DataInicio = dataInicio.Value, DataFim = dataFim.Value };
                }

                sql += " ORDER BY c.dataHora DESC";

                var colheitas = await connection.QueryAsync<RelatorioColheitaDto>(sql, parameters);

                return new RelatorioColheitaResponseDto
                {
                    Success = true,
                    Data = colheitas.ToList(),
                    TotalRegistros = colheitas.Count()
                };
            }
            catch (Exception ex)
            {
                return new RelatorioColheitaResponseDto
                {
                    Success = false,
                    Error = $"Erro ao gerar relatório de colheitas: {ex.Message}"
                };
            }
        }

        public async Task<RelatorioMovimentacaoEstoqueResponseDto> GerarRelatorioMovimentacaoEstoqueAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
                
                var sql = @"
                    SELECT 
                        me.Id,
                        me.movimentacao as TipoMovimentacao,
                        COALESCE(ag.nome, s.nome, i.nome) as Produto,
                        me.qtde as Quantidade,
                        COALESCE(ag.unidade_Medida, 'unidade') as Unidade,
                        me.dataHora as Data,
                        me.descricao as Observacoes
                    FROM MovimentacoesEstoque me
                    INNER JOIN Lavouras l ON me.lavouraID = l.Id
                    LEFT JOIN Agrotoxicos ag ON me.agrotoxicoID = ag.Id
                    LEFT JOIN Sementes s ON me.sementeID = s.Id
                    LEFT JOIN Insumos i ON me.insumoID = i.Id
                    WHERE l.UsuarioId = @UsuarioId AND me.lavouraID = @LavouraId";

                object parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId };

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    sql += " AND me.dataHora BETWEEN @DataInicio AND @DataFim";
                    parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId, DataInicio = dataInicio.Value, DataFim = dataFim.Value };
                }

                sql += " ORDER BY me.dataHora DESC";

                var movimentacoes = await connection.QueryAsync<RelatorioMovimentacaoEstoqueDto>(sql, parameters);

                return new RelatorioMovimentacaoEstoqueResponseDto
                {
                    Success = true,
                    Data = movimentacoes.ToList(),
                    TotalRegistros = movimentacoes.Count()
                };
            }
            catch (Exception ex)
            {
                return new RelatorioMovimentacaoEstoqueResponseDto
                {
                    Success = false,
                    Error = $"Erro ao gerar relatório de movimentação de estoque: {ex.Message}"
                };
            }
        }

        public async Task<RelatorioPlantioResponseDto> GerarRelatorioPlantioAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
                
                var sql = @"
                    SELECT 
                        p.Id,
                        s.nome as Cultura,
                        l.nome as Lavoura,
                        p.dataHora as DataPlantio,
                        p.areaPlantada as AreaPlantada,
                        'Ativo' as Status,
                        p.descricao as Observacoes
                    FROM Plantios p
                    INNER JOIN Lavouras l ON p.lavouraID = l.Id
                    INNER JOIN Sementes s ON p.sementeID = s.Id
                    WHERE l.UsuarioId = @UsuarioId AND p.lavouraID = @LavouraId";

                object parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId };

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    sql += " AND p.dataHora BETWEEN @DataInicio AND @DataFim";
                    parameters = new { UsuarioId = usuarioId, LavouraId = lavouraId, DataInicio = dataInicio.Value, DataFim = dataFim.Value };
                }

                sql += " ORDER BY p.dataHora DESC";

                var plantios = await connection.QueryAsync<RelatorioPlantioDto>(sql, parameters);

                return new RelatorioPlantioResponseDto
                {
                    Success = true,
                    Data = plantios.ToList(),
                    TotalRegistros = plantios.Count()
                };
            }
            catch (Exception ex)
            {
                return new RelatorioPlantioResponseDto
                {
                    Success = false,
                    Error = $"Erro ao gerar relatório de plantios: {ex.Message}"
                };
            }
        }
    }
}
