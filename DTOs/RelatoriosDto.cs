namespace API_TCC.DTOs
{
    public class RelatorioAplicacaoDTO
    {
        public int Id { get; set; }
        public string Lavoura { get; set; }
        public string Agrotoxico { get; set; }
        public string Fornecedor { get; set; }
        public double Quantidade { get; set; }
        public DateTime DataHora { get; set; }
        public string? Observacao { get; set; }
    }

    public class RelatorioAplicacaoInsumoDTO
    {
        public int Id { get; set; }
        public string Lavoura { get; set; }
        public string Insumo { get; set; }
        public string Fornecedor { get; set; }
        public double Quantidade { get; set; }
        public DateTime DataHora { get; set; }
    }

    public class RelatorioPlantioDTO
    {
        public int Id { get; set; }
        public string Lavoura { get; set; }
        public string Semente { get; set; }
        public string Fornecedor { get; set; }
        public double AreaPlantada { get; set; }
        public double Quantidade { get; set; }
        public DateTime DataHora { get; set; }
    }

    public class RelatorioColheitaDTO
    {
        public int Id { get; set; }
        public string Lavoura { get; set; }
        public double QuantidadeSacas { get; set; }
        public double AreaHa { get; set; }
        public string CooperativaDestino { get; set; }
        public double PrecoSaca { get; set; }
        public DateTime DataHora { get; set; }
    }

    public class RelatorioMovimentacaoDTO
    {
        public int Id { get; set; }
        public string Lavoura { get; set; }
        public string Item { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public double Quantidade { get; set; }
        public DateTime DataHora { get; set; }
    }
}
