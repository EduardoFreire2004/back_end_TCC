namespace API_TCC.DTOs
{
    public class RelatorioRequestDto
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class RelatorioResponseDto
    {
        public bool Success { get; set; }
        public object? Relatorio { get; set; }
        public string? Error { get; set; }
    }

    // Relatórios específicos para cada tipo de produto
    public class RelatorioAgrotoxicosDto
    {
        public LavouraInfoDto Lavoura { get; set; } = new();
        public List<AgrotoxicoRelatorioDto> Agrotoxicos { get; set; } = new();
        public EstatisticasAgrotoxicosDto Estatisticas { get; set; } = new();
        public PeriodoDto Periodo { get; set; } = new();
    }

    public class RelatorioInsumosDto
    {
        public LavouraInfoDto Lavoura { get; set; } = new();
        public List<InsumoRelatorioDto> Insumos { get; set; } = new();
        public EstatisticasInsumosDto Estatisticas { get; set; } = new();
        public PeriodoDto Periodo { get; set; } = new();
    }

    public class RelatorioSementesDto
    {
        public LavouraInfoDto Lavoura { get; set; } = new();
        public List<SementeRelatorioDto> Sementes { get; set; } = new();
        public EstatisticasSementesDto Estatisticas { get; set; } = new();
        public PeriodoDto Periodo { get; set; } = new();
    }

    // Relatórios existentes
    public class RelatorioGeralDto
    {
        public LavouraInfoDto Lavoura { get; set; } = new();
        public ResumoGeralDto Resumo { get; set; } = new();
        public PeriodoDto Periodo { get; set; } = new();
    }

    public class RelatorioPlantiosDto
    {
        public LavouraInfoDto Lavoura { get; set; } = new();
        public List<PlantioRelatorioDto> Plantios { get; set; } = new();
        public EstatisticasPlantiosDto Estatisticas { get; set; } = new();
    }

    public class RelatorioAplicacoesDto
    {
        public LavouraInfoDto Lavoura { get; set; } = new();
        public List<AplicacaoRelatorioDto> Aplicacoes { get; set; } = new();
        public EstatisticasAplicacoesDto Estatisticas { get; set; } = new();
    }

    public class RelatorioColheitasDto
    {
        public LavouraInfoDto Lavoura { get; set; } = new();
        public List<ColheitaRelatorioDto> Colheitas { get; set; } = new();
        public EstatisticasColheitasDto Estatisticas { get; set; } = new();
    }

    public class RelatorioCustosDto
    {
        public LavouraInfoDto Lavoura { get; set; } = new();
        public List<CustoRelatorioDto> Custos { get; set; } = new();
        public EstatisticasCustosDto Estatisticas { get; set; } = new();
    }

    public class RelatorioEstoqueDto
    {
        public LavouraInfoDto Lavoura { get; set; } = new();
        public List<MovimentacaoRelatorioDto> Movimentacoes { get; set; } = new();
        public Dictionary<string, decimal> SaldoAtual { get; set; } = new();
        public EstatisticasEstoqueDto Estatisticas { get; set; } = new();
    }

    // DTOs auxiliares
    public class LavouraInfoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal? Area { get; set; }
        public string? Status { get; set; }
    }

    public class PeriodoDto
    {
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }

    // DTOs específicos para agrotoxicos
    public class AgrotoxicoRelatorioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Fornecedor { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal Preco { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public decimal ValorTotal { get; set; }
        public int TotalAplicacoes { get; set; }
        public decimal AreaTotalAplicada { get; set; }
    }

    public class EstatisticasAgrotoxicosDto
    {
        public int TotalAgrotoxicos { get; set; }
        public decimal ValorTotalEstoque { get; set; }
        public decimal QuantidadeTotal { get; set; }
        public List<string> Tipos { get; set; } = new();
        public List<string> Fornecedores { get; set; } = new();
        public int TotalAplicacoes { get; set; }
        public decimal AreaTotalAplicada { get; set; }
        public decimal CustoMedioPorHectare { get; set; }
    }

    // DTOs específicos para insumos
    public class InsumoRelatorioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Fornecedor { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal Preco { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public decimal ValorTotal { get; set; }
        public int TotalAplicacoes { get; set; }
        public decimal AreaTotalAplicada { get; set; }
    }

    public class EstatisticasInsumosDto
    {
        public int TotalInsumos { get; set; }
        public decimal ValorTotalEstoque { get; set; }
        public decimal QuantidadeTotal { get; set; }
        public List<string> Categorias { get; set; } = new();
        public List<string> Fornecedores { get; set; } = new();
        public int TotalAplicacoes { get; set; }
        public decimal AreaTotalAplicada { get; set; }
        public decimal CustoMedioPorHectare { get; set; }
    }

    // DTOs específicos para sementes
    public class SementeRelatorioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cultura { get; set; } = string.Empty;
        public string Fornecedor { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal Preco { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public decimal ValorTotal { get; set; }
        public int TotalPlantios { get; set; }
        public decimal AreaTotalPlantada { get; set; }
    }

    public class EstatisticasSementesDto
    {
        public int TotalSementes { get; set; }
        public decimal ValorTotalEstoque { get; set; }
        public decimal QuantidadeTotal { get; set; }
        public List<string> Culturas { get; set; } = new();
        public List<string> Fornecedores { get; set; } = new();
        public int TotalPlantios { get; set; }
        public decimal AreaTotalPlantada { get; set; }
        public decimal CustoMedioPorHectare { get; set; }
    }

    public class ResumoGeralDto
    {
        public int TotalPlantios { get; set; }
        public int TotalAplicacoesAgrotoxicos { get; set; }
        public int TotalAplicacoesInsumos { get; set; }
        public int TotalColheitas { get; set; }
        public decimal CustoTotal { get; set; }
        public decimal ReceitaTotal { get; set; }
        public decimal LucroEstimado { get; set; }
    }

    public class PlantioRelatorioDto
    {
        public int Id { get; set; }
        public string Cultura { get; set; } = string.Empty;
        public decimal AreaPlantada { get; set; }
        public DateTime DataPlantio { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
    }

    public class EstatisticasPlantiosDto
    {
        public int TotalPlantios { get; set; }
        public decimal AreaTotalPlantada { get; set; }
        public List<string> Culturas { get; set; } = new();
        public string PeriodoAnalisado { get; set; } = string.Empty;
    }

    public class AplicacaoRelatorioDto
    {
        public int Id { get; set; }
        public string Produto { get; set; } = string.Empty;
        public decimal AreaAplicada { get; set; }
        public DateTime DataAplicacao { get; set; }
        public string Dosagem { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
    }

    public class EstatisticasAplicacoesDto
    {
        public int TotalAplicacoes { get; set; }
        public decimal AreaTotalAplicada { get; set; }
        public List<string> ProdutosUtilizados { get; set; } = new();
        public decimal CustoTotal { get; set; }
    }

    public class ColheitaRelatorioDto
    {
        public int Id { get; set; }
        public string Cultura { get; set; } = string.Empty;
        public decimal AreaColhida { get; set; }
        public DateTime DataColheita { get; set; }
        public decimal Produtividade { get; set; }
        public decimal QuantidadeColhida { get; set; }
        public string? Observacoes { get; set; }
    }

    public class EstatisticasColheitasDto
    {
        public int TotalColheitas { get; set; }
        public decimal AreaTotalColhida { get; set; }
        public decimal ProdutividadeMedia { get; set; }
        public decimal QuantidadeTotal { get; set; }
        public decimal ReceitaEstimada { get; set; }
    }

    public class CustoRelatorioDto
    {
        public int Id { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string? Observacoes { get; set; }
    }

    public class EstatisticasCustosDto
    {
        public decimal TotalCustos { get; set; }
        public decimal CustoPorHectare { get; set; }
        public Dictionary<string, decimal> Categorias { get; set; } = new();
        public string PeriodoAnalisado { get; set; } = string.Empty;
    }

    public class MovimentacaoRelatorioDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Produto { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string? Observacoes { get; set; }
    }

    public class EstatisticasEstoqueDto
    {
        public int TotalEntradas { get; set; }
        public int TotalSaidas { get; set; }
        public int ProdutosEmEstoque { get; set; }
        public decimal ValorEstoque { get; set; }
    }
}
