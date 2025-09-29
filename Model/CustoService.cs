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
                    
                    // Adicionar custo operacional (estimativa de 10% do valor do produto)
                    var custoOperacional = custoAplicacao * 0.1;
                    custoCalculado.CustoAplicacoes += custoOperacional;

                    custoCalculado.Detalhes.Add(new DetalheCustoDto
                    {
                        Categoria = "Aplicação de Agrotóxico",
                        Descricao = aplicacao.descricao,
                        Custo = custoAplicacao + custoOperacional,
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
                    
                    // Adicionar custo operacional (estimativa de 10% do valor do produto)
                    var custoOperacional = custoInsumo * 0.1;
                    custoCalculado.CustoAplicacoesInsumos += custoOperacional;

                    custoCalculado.Detalhes.Add(new DetalheCustoDto
                    {
                        Categoria = "Aplicação de Insumo",
                        Descricao = aplicacaoInsumo.descricao,
                        Custo = custoInsumo + custoOperacional,
                        Data = aplicacaoInsumo.dataHora,
                        ProdutoNome = aplicacaoInsumo.insumo.nome,
                        Quantidade = aplicacaoInsumo.qtde,
                        UnidadeMedida = aplicacaoInsumo.insumo.unidade_Medida
                    });
                }

                // 3. Calcular custos de movimentações de estoque (saídas)
                var movimentacoesQuery = _contexto.MovimentacoesEstoque
                    .Where(m => m.lavouraID == lavouraId && m.UsuarioId == usuarioId && m.movimentacao == Enums.TipoMovimentacao.Saida);

                if (filtroData)
                {
                    movimentacoesQuery = movimentacoesQuery.Where(m => m.dataHora >= dataInicio.Value && m.dataHora <= dataFim.Value);
                }

                var movimentacoes = await movimentacoesQuery.ToListAsync();
                foreach (var movimentacao in movimentacoes)
                {
                    double custoProduto = 0;
                    string? produtoNome = null;
                    string? produtoTipo = null;
                    double? quantidade = movimentacao.qtde;
                    string? unidadeMedida = null;

                    if (movimentacao.agrotoxicoID.HasValue)
                    {
                        var agrotoxico = await _contexto.Agrotoxicos.FindAsync(movimentacao.agrotoxicoID.Value);
                        if (agrotoxico != null)
                        {
                            custoProduto = movimentacao.qtde * (double)agrotoxico.preco;
                            produtoNome = agrotoxico.nome;
                            produtoTipo = "Agrotóxico";
                            unidadeMedida = agrotoxico.unidade_Medida;
                        }
                    }
                    else if (movimentacao.insumoID.HasValue)
                    {
                        var insumo = await _contexto.Insumos.FindAsync(movimentacao.insumoID.Value);
                        if (insumo != null)
                        {
                            custoProduto = movimentacao.qtde * (double)insumo.preco;
                            produtoNome = insumo.nome;
                            produtoTipo = "Insumo";
                            unidadeMedida = insumo.unidade_Medida;
                        }
                    }
                    else if (movimentacao.sementeID.HasValue)
                    {
                        var semente = await _contexto.Sementes.FindAsync(movimentacao.sementeID.Value);
                        if (semente != null)
                        {
                            custoProduto = movimentacao.qtde * (double)semente.preco;
                            produtoNome = semente.nome;
                            produtoTipo = "Semente";
                            unidadeMedida = "kg";
                        }
                    }

                    custoCalculado.CustoMovimentacoes += custoProduto;

                    custoCalculado.Detalhes.Add(new DetalheCustoDto
                    {
                        Categoria = "Movimentação de Estoque",
                        Descricao = movimentacao.descricao ?? $"Saída de {produtoTipo}",
                        Custo = custoProduto,
                        Data = movimentacao.dataHora,
                        ProdutoNome = produtoNome,
                        Quantidade = quantidade,
                        UnidadeMedida = unidadeMedida
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

                // 5. Calcular custos de colheitas (se houver custos operacionais)
                var colheitasQuery = _contexto.Colheitas
                    .Where(c => c.lavouraID == lavouraId && c.UsuarioId == usuarioId);

                if (filtroData)
                {
                    colheitasQuery = colheitasQuery.Where(c => c.dataHora >= dataInicio.Value && c.dataHora <= dataFim.Value);
                }

                var colheitas = await colheitasQuery.ToListAsync();
                foreach (var colheita in colheitas)
                {
                    // Estimativa de custo operacional de colheita (R$ 50 por hectare)
                    var custoColheita = colheita.areaHectares * 50.0;
                    custoCalculado.CustoColheitas += custoColheita;

                    custoCalculado.Detalhes.Add(new DetalheCustoDto
                    {
                        Categoria = "Colheita",
                        Descricao = colheita.descricao,
                        Custo = custoColheita,
                        Data = colheita.dataHora,
                        ProdutoNome = null,
                        Quantidade = colheita.areaHectares,
                        UnidadeMedida = "ha"
                    });
                }

                // Calcular custo total
                custoCalculado.CustoTotal = custoCalculado.CustoAplicacoes + 
                                          custoCalculado.CustoAplicacoesInsumos + 
                                          custoCalculado.CustoMovimentacoes + 
                                          custoCalculado.CustoPlantios + 
                                          custoCalculado.CustoColheitas;

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
                var custoOperacional = custoAgrotoxico * 0.1; // 10% do valor do produto

                return new CustoAplicacaoDto
                {
                    AplicacaoId = aplicacao.Id,
                    LavouraId = aplicacao.lavouraID,
                    NomeLavoura = aplicacao.lavoura.nome,
                    DescricaoAplicacao = aplicacao.descricao,
                    CustoTotal = custoAgrotoxico + custoOperacional,
                    CustoAgrotoxico = custoAgrotoxico,
                    CustoOperacional = custoOperacional,
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
                var custoOperacional = custoInsumo * 0.1; // 10% do valor do produto

                return new CustoAplicacaoInsumoDto
                {
                    AplicacaoInsumoId = aplicacaoInsumo.Id,
                    LavouraId = aplicacaoInsumo.lavouraID,
                    NomeLavoura = aplicacaoInsumo.lavoura.nome,
                    DescricaoAplicacao = aplicacaoInsumo.descricao,
                    CustoTotal = custoInsumo + custoOperacional,
                    CustoInsumo = custoInsumo,
                    CustoOperacional = custoOperacional,
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

        public async Task<CustoMovimentacaoDto> CalcularCustoMovimentacaoAsync(int movimentacaoId, int usuarioId)
        {
            try
            {
                var movimentacao = await _contexto.MovimentacoesEstoque
                    .Include(m => m.Lavoura)
                    .Where(m => m.Id == movimentacaoId && m.UsuarioId == usuarioId)
                    .FirstOrDefaultAsync();

                if (movimentacao == null)
                    throw new InvalidOperationException("Movimentação não encontrada");

                double custoProduto = 0;
                string? produtoNome = null;
                string? produtoTipo = null;

                if (movimentacao.agrotoxicoID.HasValue)
                {
                    var agrotoxico = await _contexto.Agrotoxicos.FindAsync(movimentacao.agrotoxicoID.Value);
                    if (agrotoxico != null)
                    {
                        custoProduto = movimentacao.qtde * (double)agrotoxico.preco;
                        produtoNome = agrotoxico.nome;
                        produtoTipo = "Agrotóxico";
                    }
                }
                else if (movimentacao.insumoID.HasValue)
                {
                    var insumo = await _contexto.Insumos.FindAsync(movimentacao.insumoID.Value);
                    if (insumo != null)
                    {
                        custoProduto = movimentacao.qtde * (double)insumo.preco;
                        produtoNome = insumo.nome;
                        produtoTipo = "Insumo";
                    }
                }
                else if (movimentacao.sementeID.HasValue)
                {
                    var semente = await _contexto.Sementes.FindAsync(movimentacao.sementeID.Value);
                    if (semente != null)
                    {
                        custoProduto = movimentacao.qtde * (double)semente.preco;
                        produtoNome = semente.nome;
                        produtoTipo = "Semente";
                    }
                }

                var custoOperacional = custoProduto * 0.05; // 5% do valor do produto

                return new CustoMovimentacaoDto
                {
                    MovimentacaoId = movimentacao.Id,
                    LavouraId = movimentacao.lavouraID,
                    NomeLavoura = movimentacao.Lavoura.nome,
                    TipoMovimentacao = movimentacao.movimentacao.ToString(),
                    CustoTotal = custoProduto + custoOperacional,
                    CustoProduto = custoProduto,
                    CustoOperacional = custoOperacional,
                    DataMovimentacao = movimentacao.dataHora,
                    Descricao = movimentacao.descricao ?? $"Movimentação de {produtoTipo}",
                    Quantidade = movimentacao.qtde,
                    ProdutoNome = produtoNome,
                    ProdutoTipo = produtoTipo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular custo da movimentação {MovimentacaoId}", movimentacaoId);
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

                var totalMovimentacoes = await _contexto.MovimentacoesEstoque
                    .Where(m => m.lavouraID == lavouraId && m.UsuarioId == usuarioId && 
                               m.dataHora >= dataInicio && m.dataHora <= dataFim)
                    .CountAsync();

                var totalPlantios = await _contexto.Plantios
                    .Where(p => p.lavouraID == lavouraId && p.UsuarioId == usuarioId && 
                               p.dataHora >= dataInicio && p.dataHora <= dataFim)
                    .CountAsync();

                var totalColheitas = await _contexto.Colheitas
                    .Where(c => c.lavouraID == lavouraId && c.UsuarioId == usuarioId && 
                               c.dataHora >= dataInicio && c.dataHora <= dataFim)
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
                    TotalMovimentacoes = totalMovimentacoes,
                    TotalPlantios = totalPlantios,
                    TotalColheitas = totalColheitas
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







