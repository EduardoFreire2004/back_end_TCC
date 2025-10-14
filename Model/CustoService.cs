using API_TCC.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API_TCC.Model
{
    public class CustoService : ICustoService
    {
        private readonly Contexto _contexto;
        private readonly ILogger<CustoService> _logger;

        public CustoService(Contexto contexto, ILogger<CustoService> logger)
        {
            _contexto = contexto;
            _logger = logger;
        }

        public async Task<CustoCalculadoDto> CalcularCustosLavouraAsync(int lavouraId, int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                _logger.LogInformation("Calculando custos para lavoura {LavouraId} do usuário {UsuarioId}", lavouraId, usuarioId);

                var lavoura = await _contexto.Lavouras
                    .Where(l => l.Id == lavouraId && l.UsuarioId == usuarioId)
                    .FirstOrDefaultAsync();

                if (lavoura == null)
                {
                    _logger.LogWarning("Lavoura {LavouraId} não encontrada para usuário {UsuarioId}", lavouraId, usuarioId);
                    throw new InvalidOperationException("Lavoura não encontrada");
                }

                var custoCalculado = new CustoCalculadoDto
                {
                    LavouraId = lavouraId,
                    NomeLavoura = lavoura.nome,
                    DataCalculo = DateTime.UtcNow
                };

                // Filtrar por período se especificado
                var filtroData = dataInicio.HasValue && dataFim.HasValue;

                // 1. Calcular custos de aplicações de agrotóxicos
                var aplicacoesQuery = _contexto.Aplicacoes
                    .Include(a => a.agrotoxico)
                    .Where(a => a.lavouraID == lavouraId && a.UsuarioId == usuarioId);

                if (filtroData)
                {
                    aplicacoesQuery = aplicacoesQuery.Where(a => a.dataHora >= dataInicio.Value && a.dataHora <= dataFim.Value);
                }

                var aplicacoes = await aplicacoesQuery.ToListAsync();
                foreach (var aplicacao in aplicacoes)
                {
                    var custoAplicacao = aplicacao.qtde * (double)aplicacao.agrotoxico.preco;
                    custoCalculado.CustoAplicacoes += custoAplicacao;
                    
                    custoCalculado.Detalhes.Add(new DetalheCustoDto
                    {
                        Categoria = "Aplicação de Agrotóxico",
                        Descricao = aplicacao.descricao,
                        Custo = custoAplicacao,
                        Data = aplicacao.dataHora,
                        ProdutoNome = aplicacao.agrotoxico.nome,
                        Quantidade = aplicacao.qtde,
                        UnidadeMedida = aplicacao.agrotoxico.unidade_Medida
                    });
                }

                // 2. Calcular custos de aplicações de insumos
                var aplicacoesInsumosQuery = _contexto.Aplicacao_Insumos
                    .Include(ai => ai.insumo)
                    .Where(ai => ai.lavouraID == lavouraId && ai.UsuarioId == usuarioId);

                if (filtroData)
                {
                    aplicacoesInsumosQuery = aplicacoesInsumosQuery.Where(ai => ai.dataHora >= dataInicio.Value && ai.dataHora <= dataFim.Value);
                }

                var aplicacoesInsumos = await aplicacoesInsumosQuery.ToListAsync();
                foreach (var aplicacaoInsumo in aplicacoesInsumos)
                {
                    var custoInsumo = aplicacaoInsumo.qtde * (double)aplicacaoInsumo.insumo.preco;
                    custoCalculado.CustoAplicacoesInsumos += custoInsumo;
                    
                    custoCalculado.Detalhes.Add(new DetalheCustoDto
                    {
                        Categoria = "Aplicação de Insumo",
                        Descricao = aplicacaoInsumo.descricao,
                        Custo = custoInsumo,
                        Data = aplicacaoInsumo.dataHora,
                        ProdutoNome = aplicacaoInsumo.insumo.nome,
                        Quantidade = aplicacaoInsumo.qtde,
                        UnidadeMedida = aplicacaoInsumo.insumo.unidade_Medida
                    });
                }

                // 4. Calcular custos de plantios
                var plantiosQuery = _contexto.Plantios
                    .Include(p => p.semente)
                    .Where(p => p.lavouraID == lavouraId && p.UsuarioId == usuarioId);

                if (filtroData)
                {
                    plantiosQuery = plantiosQuery.Where(p => p.dataHora >= dataInicio.Value && p.dataHora <= dataFim.Value);
                }

                var plantios = await plantiosQuery.ToListAsync();
                foreach (var plantio in plantios)
                {
                    var custoPlantio = plantio.qtde * (double)plantio.semente.preco;
                    custoCalculado.CustoPlantios += custoPlantio;

                    custoCalculado.Detalhes.Add(new DetalheCustoDto
                    {
                        Categoria = "Plantio",
                        Descricao = plantio.descricao,
                        Custo = custoPlantio,
                        Data = plantio.dataHora,
                        ProdutoNome = plantio.semente.nome,
                        Quantidade = plantio.qtde,
                        UnidadeMedida = "kg"
                    });
                }

                // Calcular custo total
                custoCalculado.CustoTotal = custoCalculado.CustoAplicacoes +
                                          custoCalculado.CustoAplicacoesInsumos +
                                          custoCalculado.CustoPlantios;

                _logger.LogInformation("Custos calculados para lavoura {LavouraId}: Total R$ {CustoTotal:F2}", lavouraId, custoCalculado.CustoTotal);

                return custoCalculado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular custos para lavoura {LavouraId}", lavouraId);
                throw;
            }
        }

        public async Task<CustoAplicacaoDto> CalcularCustoAplicacaoAsync(int aplicacaoId, int usuarioId)
        {
            try
            {
                var aplicacao = await _contexto.Aplicacoes
                    .Include(a => a.agrotoxico)
                    .Include(a => a.lavoura)
                    .Where(a => a.Id == aplicacaoId && a.UsuarioId == usuarioId)
                    .FirstOrDefaultAsync();

                if (aplicacao == null)
                    throw new InvalidOperationException("Aplicação não encontrada");

                var custoAgrotoxico = aplicacao.qtde * (double)aplicacao.agrotoxico.preco;

                return new CustoAplicacaoDto
                {
                    AplicacaoId = aplicacao.Id,
                    LavouraId = aplicacao.lavouraID,
                    NomeLavoura = aplicacao.lavoura.nome,
                    DescricaoAplicacao = aplicacao.descricao,
                    CustoTotal = custoAgrotoxico,
                    CustoAgrotoxico = custoAgrotoxico,
                    DataAplicacao = aplicacao.dataHora,
                    AgrotoxicoNome = aplicacao.agrotoxico.nome,
                    Quantidade = aplicacao.qtde,
                    UnidadeMedida = aplicacao.agrotoxico.unidade_Medida
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular custo da aplicação {AplicacaoId}", aplicacaoId);
                throw;
            }
        }

        public async Task<CustoAplicacaoInsumoDto> CalcularCustoAplicacaoInsumoAsync(int aplicacaoInsumoId, int usuarioId)
        {
            try
            {
                var aplicacaoInsumo = await _contexto.Aplicacao_Insumos
                    .Include(ai => ai.insumo)
                    .Include(ai => ai.lavoura)
                    .Where(ai => ai.Id == aplicacaoInsumoId && ai.UsuarioId == usuarioId)
                    .FirstOrDefaultAsync();

                if (aplicacaoInsumo == null)
                    throw new InvalidOperationException("Aplicação de insumo não encontrada");

                var custoInsumo = aplicacaoInsumo.qtde * (double)aplicacaoInsumo.insumo.preco;

                return new CustoAplicacaoInsumoDto
                {
                    AplicacaoInsumoId = aplicacaoInsumo.Id,
                    LavouraId = aplicacaoInsumo.lavouraID,
                    NomeLavoura = aplicacaoInsumo.lavoura.nome,
                    DescricaoAplicacao = aplicacaoInsumo.descricao,
                    CustoTotal = custoInsumo,
                    CustoInsumo = custoInsumo,
                    DataAplicacao = aplicacaoInsumo.dataHora,
                    InsumoNome = aplicacaoInsumo.insumo.nome,
                    Quantidade = aplicacaoInsumo.qtde,
                    UnidadeMedida = aplicacaoInsumo.insumo.unidade_Medida
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular custo da aplicação de insumo {AplicacaoInsumoId}", aplicacaoInsumoId);
                throw;
            }
        }

        public async Task<ResumoCustosDto> ObterResumoCustosAsync(int lavouraId, int usuarioId, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var custoCalculado = await CalcularCustosLavouraAsync(lavouraId, usuarioId, dataInicio, dataFim);

                // Contar operações
                var totalAplicacoes = await _contexto.Aplicacoes
                    .Where(a => a.lavouraID == lavouraId && a.UsuarioId == usuarioId && 
                               a.dataHora >= dataInicio && a.dataHora <= dataFim)
                    .CountAsync();

                var totalAplicacoesInsumos = await _contexto.Aplicacao_Insumos
                    .Where(ai => ai.lavouraID == lavouraId && ai.UsuarioId == usuarioId && 
                                ai.dataHora >= dataInicio && ai.dataHora <= dataFim)
                    .CountAsync();

                var totalPlantios = await _contexto.Plantios
                    .Where(p => p.lavouraID == lavouraId && p.UsuarioId == usuarioId && 
                               p.dataHora >= dataInicio && p.dataHora <= dataFim)
                    .CountAsync();

                return new ResumoCustosDto
                {
                    LavouraId = lavouraId,
                    NomeLavoura = custoCalculado.NomeLavoura,
                    DataInicio = dataInicio,
                    DataFim = dataFim,
                    CustoTotal = custoCalculado.CustoTotal,
                    CustoAplicacoes = custoCalculado.CustoAplicacoes,
                    CustoAplicacoesInsumos = custoCalculado.CustoAplicacoesInsumos,
                    CustoMovimentacoes = custoCalculado.CustoMovimentacoes,
                    CustoPlantios = custoCalculado.CustoPlantios,
                    CustoColheitas = custoCalculado.CustoColheitas,
                    TotalAplicacoes = totalAplicacoes,
                    TotalAplicacoesInsumos = totalAplicacoesInsumos,
                    TotalPlantios = totalPlantios,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo de custos para lavoura {LavouraId}", lavouraId);
                throw;
            }
        }

        public async Task<List<HistoricoCustoDto>> ObterHistoricoCustosAsync(int lavouraId, int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                var historico = new List<HistoricoCustoDto>();

                // Aplicações
                var aplicacoesQuery = _contexto.Aplicacoes
                    .Include(a => a.agrotoxico)
                    .Where(a => a.lavouraID == lavouraId && a.UsuarioId == usuarioId);

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    aplicacoesQuery = aplicacoesQuery.Where(a => a.dataHora >= dataInicio.Value && a.dataHora <= dataFim.Value);
                }

                var aplicacoes = await aplicacoesQuery.ToListAsync();
                foreach (var aplicacao in aplicacoes)
                {
                    var custo = aplicacao.qtde * (double)aplicacao.agrotoxico.preco;
                    historico.Add(new HistoricoCustoDto
                    {
                        Id = aplicacao.Id,
                        TipoOperacao = "Aplicação de Agrotóxico",
                        Descricao = aplicacao.descricao,
                        Custo = custo,
                        Data = aplicacao.dataHora,
                        ProdutoNome = aplicacao.agrotoxico.nome,
                        Quantidade = aplicacao.qtde,
                        UnidadeMedida = aplicacao.agrotoxico.unidade_Medida
                    });
                }

                // Aplicações de Insumos
                var aplicacoesInsumosQuery = _contexto.Aplicacao_Insumos
                    .Include(ai => ai.insumo)
                    .Where(ai => ai.lavouraID == lavouraId && ai.UsuarioId == usuarioId);

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    aplicacoesInsumosQuery = aplicacoesInsumosQuery.Where(ai => ai.dataHora >= dataInicio.Value && ai.dataHora <= dataFim.Value);
                }

                var aplicacoesInsumos = await aplicacoesInsumosQuery.ToListAsync();
                foreach (var aplicacaoInsumo in aplicacoesInsumos)
                {
                    var custo = aplicacaoInsumo.qtde * (double)aplicacaoInsumo.insumo.preco;
                    historico.Add(new HistoricoCustoDto
                    {
                        Id = aplicacaoInsumo.Id,
                        TipoOperacao = "Aplicação de Insumo",
                        Descricao = aplicacaoInsumo.descricao,
                        Custo = custo,
                        Data = aplicacaoInsumo.dataHora,
                        ProdutoNome = aplicacaoInsumo.insumo.nome,
                        Quantidade = aplicacaoInsumo.qtde,
                        UnidadeMedida = aplicacaoInsumo.insumo.unidade_Medida
                    });
                }

                // Plantios
                var plantiosQuery = _contexto.Plantios
                    .Include(p => p.semente)
                    .Where(p => p.lavouraID == lavouraId && p.UsuarioId == usuarioId);

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    plantiosQuery = plantiosQuery.Where(p => p.dataHora >= dataInicio.Value && p.dataHora <= dataFim.Value);
                }

                var plantios = await plantiosQuery.ToListAsync();
                foreach (var plantio in plantios)
                {
                    var custo = plantio.qtde * (double)plantio.semente.preco;
                    historico.Add(new HistoricoCustoDto
                    {
                        Id = plantio.Id,
                        TipoOperacao = "Plantio",
                        Descricao = plantio.descricao,
                        Custo = custo,
                        Data = plantio.dataHora,
                        ProdutoNome = plantio.semente.nome,
                        Quantidade = plantio.qtde,
                        UnidadeMedida = "kg"
                    });
                }

                return historico.OrderByDescending(h => h.Data).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter histórico de custos para lavoura {LavouraId}", lavouraId);
                throw;
            }
        }

        public async Task<bool> AtualizarCustosLavouraAsync(int lavouraId, int usuarioId)
        {
            try
            {
                _logger.LogInformation("Atualizando custos para lavoura {LavouraId}", lavouraId);

                // Recalcular custos
                var custoCalculado = await CalcularCustosLavouraAsync(lavouraId, usuarioId);

                // Atualizar ou criar registro de custo na tabela Custos
                var custoExistente = await _contexto.Custo
                    .Where(c => c.LavouraId == lavouraId && c.UsuarioId == usuarioId)
                    .FirstOrDefaultAsync();

                if (custoExistente != null)
                {
                    custoExistente.custoTotal = custoCalculado.CustoTotal;
                    custoExistente.ganhoTotal = 0; // Será calculado quando houver colheitas
                }
                else
                {
                    var novoCusto = new Custo
                    {
                        UsuarioId = usuarioId,
                        LavouraId = lavouraId,
                        custoTotal = custoCalculado.CustoTotal,
                        ganhoTotal = 0
                    };
                    _contexto.Custo.Add(novoCusto);
                }

                await _contexto.SaveChangesAsync();
                _logger.LogInformation("Custos atualizados para lavoura {LavouraId}: Total R$ {CustoTotal:F2}", lavouraId, custoCalculado.CustoTotal);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar custos para lavoura {LavouraId}", lavouraId);
                return false;
            }
        }
    }
}