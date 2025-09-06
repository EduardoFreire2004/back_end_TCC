using API_TCC.DTOs;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Data.SqlClient;

namespace API_TCC.Model
{
    public class RelatorioService : IRelatorioService
    {
        private readonly Contexto _contexto;

        public RelatorioService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<RelatorioGeralDto> GerarRelatorioGeralAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var resumo = await ObterResumoGeralAsync(lavouraId, dataInicio, dataFim);

            return new RelatorioGeralDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Resumo = resumo,
                Periodo = new PeriodoDto
                {
                    Inicio = dataInicio,
                    Fim = dataFim
                }
            };
        }

        public async Task<RelatorioAgrotoxicosDto> GerarRelatorioAgrotoxicosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var agrotoxicos = await ObterAgrotoxicosAsync(lavouraId, dataInicio, dataFim);
            var estatisticas = await ObterEstatisticasAgrotoxicosAsync(lavouraId, dataInicio, dataFim, agrotoxicos);

            return new RelatorioAgrotoxicosDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Agrotoxicos = agrotoxicos,
                Estatisticas = estatisticas
            };
        }


        public async Task<RelatorioPlantiosDto> GerarRelatorioPlantiosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var plantios = await ObterPlantiosAsync(lavouraId, dataInicio, dataFim);
            var estatisticas = await ObterEstatisticasPlantiosAsync(lavouraId, dataInicio, dataFim, plantios);

            return new RelatorioPlantiosDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Plantios = plantios,
                Estatisticas = estatisticas
            };
        }

        public async Task<RelatorioAplicacoesDto> GerarRelatorioAplicacoesAgrotoxicosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var aplicacoes = await ObterAplicacoesAgrotoxicosAsync(lavouraId, dataInicio, dataFim);
            var estatisticas = await ObterEstatisticasAplicacoesAsync(lavouraId, dataInicio, dataFim, aplicacoes);

            return new RelatorioAplicacoesDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Aplicacoes = aplicacoes,
                Estatisticas = estatisticas
            };
        }

        public async Task<RelatorioAplicacoesDto> GerarRelatorioAplicacoesInsumosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var aplicacoes = await ObterAplicacoesInsumosAsync(lavouraId, dataInicio, dataFim);
            var estatisticas = await ObterEstatisticasAplicacoesAsync(lavouraId, dataInicio, dataFim, aplicacoes);

            return new RelatorioAplicacoesDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Aplicacoes = aplicacoes,
                Estatisticas = estatisticas
            };
        }

        public async Task<RelatorioColheitasDto> GerarRelatorioColheitasAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var colheitas = await ObterColheitasAsync(lavouraId, dataInicio, dataFim);
            var estatisticas = await ObterEstatisticasColheitasAsync(lavouraId, dataInicio, dataFim, colheitas);

            return new RelatorioColheitasDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Colheitas = colheitas,
                Estatisticas = estatisticas
            };
        }

        public async Task<RelatorioCustosDto> GerarRelatorioCustosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var custos = await ObterCustosAsync(lavouraId, dataInicio, dataFim);
            var estatisticas = await ObterEstatisticasCustosAsync(lavouraId, dataInicio, dataFim, custos);

            return new RelatorioCustosDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Custos = custos,
                Estatisticas = estatisticas
            };
        }

        public async Task<RelatorioEstoqueDto> GerarRelatorioEstoqueAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var movimentacoes = await ObterMovimentacoesEstoqueAsync(lavouraId, dataInicio, dataFim);
            var saldoAtual = await ObterSaldoAtualEstoqueAsync(lavouraId);
            var estatisticas = await ObterEstatisticasEstoqueAsync(lavouraId, dataInicio, dataFim, movimentacoes);

            return new RelatorioEstoqueDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Movimentacoes = movimentacoes,
                SaldoAtual = saldoAtual,
                Estatisticas = estatisticas
            };
        }

        public async Task<RelatorioInsumosDto> GerarRelatorioInsumosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var insumos = await ObterInsumosAsync(lavouraId, dataInicio, dataFim);
            var estatisticas = await ObterEstatisticasInsumosAsync(lavouraId, dataInicio, dataFim, insumos);

            return new RelatorioInsumosDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Insumos = insumos,
                Estatisticas = estatisticas
            };
        }

        public async Task<RelatorioSementesDto> GerarRelatorioSementesAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            if (lavoura == null)
                throw new ArgumentException("Lavoura não encontrada");

            var sementes = await ObterSementesAsync(lavouraId, dataInicio, dataFim);
            var estatisticas = await ObterEstatisticasSementesAsync(lavouraId, dataInicio, dataFim, sementes);

            return new RelatorioSementesDto
            {
                Lavoura = new LavouraInfoDto
                {
                    Id = lavoura.Id,
                    Nome = lavoura.nome,
                    Area = (decimal?)lavoura.area,
                    Status = "Ativa"
                },
                Sementes = sementes,
                Estatisticas = estatisticas
            };
        }


        #region Métodos auxiliares para consultas

        private async Task<ResumoGeralDto> ObterResumoGeralAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
            
            var sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM Plantios WHERE LavouraId = @LavouraId AND DataPlantio BETWEEN @DataInicio AND @DataFim) as TotalPlantios,
                    (SELECT COUNT(*) FROM Aplicacoes WHERE LavouraId = @LavouraId AND DataAplicacao BETWEEN @DataInicio AND @DataFim) as TotalAplicacoesAgrotoxicos,
                    (SELECT COUNT(*) FROM AplicacaoInsumos WHERE LavouraId = @LavouraId AND DataAplicacao BETWEEN @DataInicio AND @DataFim) as TotalAplicacoesInsumos,
                    (SELECT COUNT(*) FROM Colheitas WHERE LavouraId = @LavouraId AND DataColheita BETWEEN @DataInicio AND @DataFim) as TotalColheitas,
                    (SELECT ISNULL(SUM(Valor), 0) FROM Custos WHERE LavouraId = @LavouraId AND Data BETWEEN @DataInicio AND @DataFim) as CustoTotal";

            var resumo = await connection.QueryFirstOrDefaultAsync<ResumoGeralDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            
            if (resumo == null)
                resumo = new ResumoGeralDto();

            // Calcular receita e lucro estimado (implementar conforme regras de negócio)
            resumo.ReceitaTotal = 0; // Implementar cálculo
            resumo.LucroEstimado = resumo.ReceitaTotal - resumo.CustoTotal;

            return resumo;
        }

        private async Task<List<PlantioRelatorioDto>> ObterPlantiosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
            
            var sql = @"
                SELECT Id, Cultura, AreaPlantada, DataPlantio, Status, Observacoes
                FROM Plantios 
                WHERE LavouraId = @LavouraId AND DataPlantio BETWEEN @DataInicio AND @DataFim
                ORDER BY DataPlantio DESC";

            var plantios = await connection.QueryAsync<PlantioRelatorioDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            return plantios.ToList();
        }

        private async Task<EstatisticasPlantiosDto> ObterEstatisticasPlantiosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim, List<PlantioRelatorioDto> plantios)
        {
            var estatisticas = new EstatisticasPlantiosDto
            {
                TotalPlantios = plantios.Count,
                AreaTotalPlantada = plantios.Sum(p => p.AreaPlantada),
                Culturas = plantios.Select(p => p.Cultura).Distinct().ToList(),
                PeriodoAnalisado = $"{dataInicio:MMMM yyyy} a {dataFim:MMMM yyyy}"
            };

            return estatisticas;
        }

        private async Task<List<AplicacaoRelatorioDto>> ObterAplicacoesAgrotoxicosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
            
            var sql = @"
                SELECT a.Id, a.Produto, a.AreaAplicada, a.DataAplicacao, a.Dosagem, a.Observacoes
                FROM Aplicacoes a
                WHERE a.LavouraId = @LavouraId AND a.DataAplicacao BETWEEN @DataInicio AND @DataFim
                ORDER BY a.DataAplicacao DESC";

            var aplicacoes = await connection.QueryAsync<AplicacaoRelatorioDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            return aplicacoes.ToList();
        }

        private async Task<List<AplicacaoRelatorioDto>> ObterAplicacoesInsumosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
            
            var sql = @"
                SELECT a.Id, i.Nome as Produto, a.AreaAplicada, a.DataAplicacao, a.Dosagem, a.Observacoes
                FROM AplicacaoInsumos a
                INNER JOIN Insumos i ON a.InsumoId = i.Id
                WHERE a.LavouraId = @LavouraId AND a.DataAplicacao BETWEEN @DataInicio AND @DataFim
                ORDER BY a.DataAplicacao DESC";

            var aplicacoes = await connection.QueryAsync<AplicacaoRelatorioDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            return aplicacoes.ToList();
        }

        private async Task<EstatisticasAplicacoesDto> ObterEstatisticasAplicacoesAsync(int lavouraId, DateTime dataInicio, DateTime dataFim, List<AplicacaoRelatorioDto> aplicacoes)
        {
            var estatisticas = new EstatisticasAplicacoesDto
            {
                TotalAplicacoes = aplicacoes.Count,
                AreaTotalAplicada = aplicacoes.Sum(a => a.AreaAplicada),
                ProdutosUtilizados = aplicacoes.Select(a => a.Produto).Distinct().ToList(),
                CustoTotal = 0 // Implementar cálculo de custo
            };

            return estatisticas;
        }

        private async Task<List<ColheitaRelatorioDto>> ObterColheitasAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
            
            var sql = @"
                SELECT Id, Cultura, AreaColhida, DataColheita, Produtividade, QuantidadeColhida, Observacoes
                FROM Colheitas 
                WHERE LavouraId = @LavouraId AND DataColheita BETWEEN @DataInicio AND @DataFim
                ORDER BY DataColheita DESC";

            var colheitas = await connection.QueryAsync<ColheitaRelatorioDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            return colheitas.ToList();
        }

        private async Task<EstatisticasColheitasDto> ObterEstatisticasColheitasAsync(int lavouraId, DateTime dataInicio, DateTime dataFim, List<ColheitaRelatorioDto> colheitas)
        {
            var estatisticas = new EstatisticasColheitasDto
            {
                TotalColheitas = colheitas.Count,
                AreaTotalColhida = colheitas.Sum(c => c.AreaColhida),
                ProdutividadeMedia = colheitas.Count > 0 ? colheitas.Average(c => c.Produtividade) : 0,
                QuantidadeTotal = colheitas.Sum(c => c.QuantidadeColhida),
                ReceitaEstimada = 0 // Implementar cálculo de receita
            };

            return estatisticas;
        }

        private async Task<List<CustoRelatorioDto>> ObterCustosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
            
            var sql = @"
                SELECT Id, Categoria, Descricao, Valor, Data, Observacoes
                FROM Custos 
                WHERE LavouraId = @LavouraId AND Data BETWEEN @DataInicio AND @DataFim
                ORDER BY Data DESC";

            var custos = await connection.QueryAsync<CustoRelatorioDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            return custos.ToList();
        }

        private async Task<EstatisticasCustosDto> ObterEstatisticasCustosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim, List<CustoRelatorioDto> custos)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            var areaLavoura = (decimal)(lavoura?.area ?? 1.0f);

            var estatisticas = new EstatisticasCustosDto
            {
                TotalCustos = custos.Sum(c => c.Valor),
                CustoPorHectare = areaLavoura > 0 ? custos.Sum(c => c.Valor) / areaLavoura : 0,
                Categorias = custos.GroupBy(c => c.Categoria).ToDictionary(g => g.Key, g => g.Sum(c => c.Valor)),
                PeriodoAnalisado = $"{dataInicio:yyyy}"
            };

            return estatisticas;
        }

        private async Task<List<MovimentacaoRelatorioDto>> ObterMovimentacoesEstoqueAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
            
            var sql = @"
                SELECT Id, TipoMovimentacao as Tipo, Produto, Quantidade, Unidade, Data, Observacoes
                FROM MovimentacaoEstoques 
                WHERE LavouraId = @LavouraId AND Data BETWEEN @DataInicio AND @DataFim
                ORDER BY Data DESC";

            var movimentacoes = await connection.QueryAsync<MovimentacaoRelatorioDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            return movimentacoes.ToList();
        }

        private async Task<Dictionary<string, decimal>> ObterSaldoAtualEstoqueAsync(int lavouraId)
        {
            using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
            
            var sql = @"
                SELECT Produto, SUM(CASE WHEN TipoMovimentacao = 'entrada' THEN Quantidade ELSE -Quantidade END) as Saldo
                FROM MovimentacaoEstoques 
                WHERE LavouraId = @LavouraId
                GROUP BY Produto
                HAVING SUM(CASE WHEN TipoMovimentacao = 'entrada' THEN Quantidade ELSE -Quantidade END) > 0";

            var saldos = await connection.QueryAsync<dynamic>(sql, new { LavouraId = lavouraId });
            
            var saldoAtual = new Dictionary<string, decimal>();
            foreach (var saldo in saldos)
            {
                saldoAtual[saldo.Produto] = saldo.Saldo;
            }

            return saldoAtual;
        }

        private async Task<EstatisticasEstoqueDto> ObterEstatisticasEstoqueAsync(int lavouraId, DateTime dataInicio, DateTime dataFim, List<MovimentacaoRelatorioDto> movimentacoes)
        {
            var estatisticas = new EstatisticasEstoqueDto
            {
                TotalEntradas = movimentacoes.Count(m => m.Tipo.ToLower() == "entrada"),
                TotalSaidas = movimentacoes.Count(m => m.Tipo.ToLower() == "saida"),
                ProdutosEmEstoque = 0, // Implementar cálculo
                ValorEstoque = 0 // Implementar cálculo
            };

            return estatisticas;
        }

        #endregion

        #region Métodos auxiliares para novos relatórios

        private async Task<List<AgrotoxicoRelatorioDto>> ObterAgrotoxicosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
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
                    (a.qtde * a.preco) as ValorTotal,
                    (SELECT COUNT(*) FROM Aplicacoes ap WHERE ap.agrotoxicoID = a.Id AND ap.LavouraId = @LavouraId AND ap.DataAplicacao BETWEEN @DataInicio AND @DataFim) as TotalAplicacoes,
                    (SELECT ISNULL(SUM(ap.AreaAplicada), 0) FROM Aplicacoes ap WHERE ap.agrotoxicoID = a.Id AND ap.LavouraId = @LavouraId AND ap.DataAplicacao BETWEEN @DataInicio AND @DataFim) as AreaTotalAplicada
                FROM Agrotoxicos a
                INNER JOIN TipoAgrotoxicos t ON a.tipoID = t.Id
                INNER JOIN Fornecedores f ON a.fornecedorID = f.Id
                WHERE a.UsuarioId = (SELECT UsuarioId FROM Lavouras WHERE Id = @LavouraId)
                AND a.data_Cadastro BETWEEN @DataInicio AND @DataFim
                ORDER BY a.data_Cadastro DESC";

            var agrotoxicos = await connection.QueryAsync<AgrotoxicoRelatorioDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            return agrotoxicos.ToList();
        }

        private async Task<EstatisticasAgrotoxicosDto> ObterEstatisticasAgrotoxicosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim, List<AgrotoxicoRelatorioDto> agrotoxicos)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            var areaLavoura = (decimal)(lavoura?.area ?? 1.0f);

            var estatisticas = new EstatisticasAgrotoxicosDto
            {
                TotalAgrotoxicos = agrotoxicos.Count,
                ValorTotalEstoque = agrotoxicos.Sum(a => a.ValorTotal),
                QuantidadeTotal = agrotoxicos.Sum(a => a.Quantidade),
                Tipos = agrotoxicos.Select(a => a.Tipo).Distinct().ToList(),
                Fornecedores = agrotoxicos.Select(a => a.Fornecedor).Distinct().ToList(),
                TotalAplicacoes = agrotoxicos.Sum(a => a.TotalAplicacoes),
                AreaTotalAplicada = agrotoxicos.Sum(a => a.AreaTotalAplicada),
                CustoMedioPorHectare = areaLavoura > 0 ? agrotoxicos.Sum(a => a.ValorTotal) / areaLavoura : 0
            };

            return estatisticas;
        }

        private async Task<List<InsumoRelatorioDto>> ObterInsumosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
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
                    (i.qtde * i.preco) as ValorTotal,
                    (SELECT COUNT(*) FROM AplicacaoInsumos ai WHERE ai.insumoID = i.Id AND ai.lavouraID = @LavouraId AND ai.DataAplicacao BETWEEN @DataInicio AND @DataFim) as TotalAplicacoes,
                    (SELECT ISNULL(SUM(ai.AreaAplicada), 0) FROM AplicacaoInsumos ai WHERE ai.insumoID = i.Id AND ai.lavouraID = @LavouraId AND ai.DataAplicacao BETWEEN @DataInicio AND @DataFim) as AreaTotalAplicada
                FROM Insumos i
                INNER JOIN CategoriaInsumo c ON i.categoriaInsumoID = c.Id
                INNER JOIN Fornecedores f ON i.fornecedorID = f.Id
                WHERE i.UsuarioId = (SELECT UsuarioId FROM Lavouras WHERE Id = @LavouraId)
                AND i.data_Cadastro BETWEEN @DataInicio AND @DataFim
                ORDER BY i.data_Cadastro DESC";

            var insumos = await connection.QueryAsync<InsumoRelatorioDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            return insumos.ToList();
        }

        private async Task<EstatisticasInsumosDto> ObterEstatisticasInsumosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim, List<InsumoRelatorioDto> insumos)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            var areaLavoura = (decimal)(lavoura?.area ?? 1.0f);

            var estatisticas = new EstatisticasInsumosDto
            {
                TotalInsumos = insumos.Count,
                ValorTotalEstoque = insumos.Sum(i => i.ValorTotal),
                QuantidadeTotal = insumos.Sum(i => i.Quantidade),
                Categorias = insumos.Select(i => i.Categoria).Distinct().ToList(),
                Fornecedores = insumos.Select(i => i.Fornecedor).Distinct().ToList(),
                TotalAplicacoes = insumos.Sum(i => i.TotalAplicacoes),
                AreaTotalAplicada = insumos.Sum(i => i.AreaTotalAplicada),
                CustoMedioPorHectare = areaLavoura > 0 ? insumos.Sum(i => i.ValorTotal) / areaLavoura : 0
            };

            return estatisticas;
        }

        private async Task<List<SementeRelatorioDto>> ObterSementesAsync(int lavouraId, DateTime dataInicio, DateTime dataFim)
        {
            using var connection = new SqlConnection(_contexto.Database.GetConnectionString());
            
            var sql = @"
                SELECT 
                    s.Id,
                    s.nome as Nome,
                    s.cultura as Cultura,
                    f.nome as Fornecedor,
                    s.qtde as Quantidade,
                    s.preco as Preco,
                    s.unidade_Medida as UnidadeMedida,
                    s.data_Cadastro as DataCadastro,
                    (s.qtde * s.preco) as ValorTotal,
                    (SELECT COUNT(*) FROM Plantios p WHERE p.sementeID = s.Id AND p.LavouraId = @LavouraId AND p.DataPlantio BETWEEN @DataInicio AND @DataFim) as TotalPlantios,
                    (SELECT ISNULL(SUM(p.AreaPlantada), 0) FROM Plantios p WHERE p.sementeID = s.Id AND p.LavouraId = @LavouraId AND p.DataPlantio BETWEEN @DataInicio AND @DataFim) as AreaTotalPlantada
                FROM Sementes s
                INNER JOIN Fornecedores f ON s.fornecedorID = f.Id
                WHERE s.UsuarioId = (SELECT UsuarioId FROM Lavouras WHERE Id = @LavouraId)
                AND s.data_Cadastro BETWEEN @DataInicio AND @DataFim
                ORDER BY s.data_Cadastro DESC";

            var sementes = await connection.QueryAsync<SementeRelatorioDto>(sql, new { LavouraId = lavouraId, DataInicio = dataInicio, DataFim = dataFim });
            return sementes.ToList();
        }

        private async Task<EstatisticasSementesDto> ObterEstatisticasSementesAsync(int lavouraId, DateTime dataInicio, DateTime dataFim, List<SementeRelatorioDto> sementes)
        {
            var lavoura = await _contexto.Lavouras.FindAsync(lavouraId);
            var areaLavoura = (decimal)(lavoura?.area ?? 1.0f);

            var estatisticas = new EstatisticasSementesDto
            {
                TotalSementes = sementes.Count,
                ValorTotalEstoque = sementes.Sum(s => s.ValorTotal),
                QuantidadeTotal = sementes.Sum(s => s.Quantidade),
                Culturas = sementes.Select(s => s.Cultura).Distinct().ToList(),
                Fornecedores = sementes.Select(s => s.Fornecedor).Distinct().ToList(),
                TotalPlantios = sementes.Sum(s => s.TotalPlantios),
                AreaTotalPlantada = sementes.Sum(s => s.AreaTotalPlantada),
                CustoMedioPorHectare = areaLavoura > 0 ? sementes.Sum(s => s.ValorTotal) / areaLavoura : 0
            };

            return estatisticas;
        }

        #endregion
    }
}
