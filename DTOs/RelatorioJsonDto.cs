namespace API_TCC.DTOs
{
    // DTOs simplificados para relatórios em JSON
    public class RelatorioFornecedoresDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public int TotalProdutos { get; set; }
        public decimal ValorTotalProdutos { get; set; }
    }

    public class RelatorioAplicacaoDto
    {
        public int Id { get; set; }
        public string Produto { get; set; } = string.Empty;
        public string Lavoura { get; set; } = string.Empty;
        public DateTime DataAplicacao { get; set; }
        public decimal Quantidade { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
    }

    public class RelatorioAplicacaoInsumoDto
    {
        public int Id { get; set; }
        public string Insumo { get; set; } = string.Empty;
        public string Lavoura { get; set; } = string.Empty;
        public DateTime DataAplicacao { get; set; }
        public decimal Quantidade { get; set; }
        public string? Descricao { get; set; }
    }

    public class RelatorioSementeDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Fornecedor { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataCadastro { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class RelatorioInsumoDto
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
    }

    public class RelatorioAgrotoxicoDto
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
    }

    public class RelatorioColheitaDto
    {
        public int Id { get; set; }
        public string Cultura { get; set; } = string.Empty;
        public string Lavoura { get; set; } = string.Empty;
        public DateTime DataColheita { get; set; }
        public decimal QuantidadeColhida { get; set; }
        public decimal Produtividade { get; set; }
        public string? Observacoes { get; set; }
    }

    public class RelatorioMovimentacaoEstoqueDto
    {
        public int Id { get; set; }
        public string TipoMovimentacao { get; set; } = string.Empty;
        public string Produto { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string? Observacoes { get; set; }
    }

    public class RelatorioPlantioDto
    {
        public int Id { get; set; }
        public string Cultura { get; set; } = string.Empty;
        public string Lavoura { get; set; } = string.Empty;
        public DateTime DataPlantio { get; set; }
        public decimal AreaPlantada { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
    }

    // DTOs de resposta para cada tipo de relatório
    public class RelatorioFornecedoresResponseDto
    {
        public bool Success { get; set; }
        public List<RelatorioFornecedoresDto> Data { get; set; } = new();
        public int TotalRegistros { get; set; }
        public string? Error { get; set; }
    }

    public class RelatorioAplicacaoResponseDto
    {
        public bool Success { get; set; }
        public List<RelatorioAplicacaoDto> Data { get; set; } = new();
        public int TotalRegistros { get; set; }
        public string? Error { get; set; }
    }

    public class RelatorioAplicacaoInsumoResponseDto
    {
        public bool Success { get; set; }
        public List<RelatorioAplicacaoInsumoDto> Data { get; set; } = new();
        public int TotalRegistros { get; set; }
        public string? Error { get; set; }
    }

    public class RelatorioSementeResponseDto
    {
        public bool Success { get; set; }
        public List<RelatorioSementeDto> Data { get; set; } = new();
        public int TotalRegistros { get; set; }
        public string? Error { get; set; }
    }

    public class RelatorioInsumoResponseDto
    {
        public bool Success { get; set; }
        public List<RelatorioInsumoDto> Data { get; set; } = new();
        public int TotalRegistros { get; set; }
        public string? Error { get; set; }
    }

    public class RelatorioAgrotoxicoResponseDto
    {
        public bool Success { get; set; }
        public List<RelatorioAgrotoxicoDto> Data { get; set; } = new();
        public int TotalRegistros { get; set; }
        public string? Error { get; set; }
    }

    public class RelatorioColheitaResponseDto
    {
        public bool Success { get; set; }
        public List<RelatorioColheitaDto> Data { get; set; } = new();
        public int TotalRegistros { get; set; }
        public string? Error { get; set; }
    }

    public class RelatorioMovimentacaoEstoqueResponseDto
    {
        public bool Success { get; set; }
        public List<RelatorioMovimentacaoEstoqueDto> Data { get; set; } = new();
        public int TotalRegistros { get; set; }
        public string? Error { get; set; }
    }

    public class RelatorioPlantioResponseDto
    {
        public bool Success { get; set; }
        public List<RelatorioPlantioDto> Data { get; set; } = new();
        public int TotalRegistros { get; set; }
        public string? Error { get; set; }
    }
}

