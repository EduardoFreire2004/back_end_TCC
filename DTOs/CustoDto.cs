using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class CustoCalculadoDto
    {
        public int LavouraId { get; set; }
        public string NomeLavoura { get; set; } = string.Empty;
        public double CustoTotal { get; set; }
        public double CustoAplicacoes { get; set; }
        public double CustoAplicacoesInsumos { get; set; }
        public double CustoMovimentacoes { get; set; }
        public double CustoPlantios { get; set; }
        public double CustoColheitas { get; set; }
        public DateTime DataCalculo { get; set; }
        public List<DetalheCustoDto> Detalhes { get; set; } = new();
    }

    public class CustoAplicacaoDto
    {
        public int AplicacaoId { get; set; }
        public int LavouraId { get; set; }
        public string NomeLavoura { get; set; } = string.Empty;
        public string DescricaoAplicacao { get; set; } = string.Empty;
        public double CustoTotal { get; set; }
        public double CustoAgrotoxico { get; set; }
        public double CustoOperacional { get; set; }
        public DateTime DataAplicacao { get; set; }
        public string AgrotoxicoNome { get; set; } = string.Empty;
        public double Quantidade { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
    }

    public class CustoAplicacaoInsumoDto
    {
        public int AplicacaoInsumoId { get; set; }
        public int LavouraId { get; set; }
        public string NomeLavoura { get; set; } = string.Empty;
        public string DescricaoAplicacao { get; set; } = string.Empty;
        public double CustoTotal { get; set; }
        public double CustoInsumo { get; set; }
        public double CustoOperacional { get; set; }
        public DateTime DataAplicacao { get; set; }
        public string InsumoNome { get; set; } = string.Empty;
        public double Quantidade { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
    }

    public class CustoMovimentacaoDto
    {
        public int MovimentacaoId { get; set; }
        public int LavouraId { get; set; }
        public string NomeLavoura { get; set; } = string.Empty;
        public string TipoMovimentacao { get; set; } = string.Empty;
        public double CustoTotal { get; set; }
        public double CustoProduto { get; set; }
        public double CustoOperacional { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public double Quantidade { get; set; }
        public string? ProdutoNome { get; set; }
        public string? ProdutoTipo { get; set; }
    }

    public class ResumoCustosDto
    {
        public int LavouraId { get; set; }
        public string NomeLavoura { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public double CustoTotal { get; set; }
        public double CustoAplicacoes { get; set; }
        public double CustoAplicacoesInsumos { get; set; }
        public double CustoMovimentacoes { get; set; }
        public double CustoPlantios { get; set; }
        public double CustoColheitas { get; set; }
        public int TotalAplicacoes { get; set; }
        public int TotalAplicacoesInsumos { get; set; }
        public int TotalMovimentacoes { get; set; }
        public int TotalPlantios { get; set; }
        public int TotalColheitas { get; set; }
    }

    public class HistoricoCustoDto
    {
        public int Id { get; set; }
        public string TipoOperacao { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public double Custo { get; set; }
        public DateTime Data { get; set; }
        public string? ProdutoNome { get; set; }
        public double? Quantidade { get; set; }
        public string? UnidadeMedida { get; set; }
    }

    public class DetalheCustoDto
    {
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public double Custo { get; set; }
        public DateTime Data { get; set; }
        public string? ProdutoNome { get; set; }
        public double? Quantidade { get; set; }
        public string? UnidadeMedida { get; set; }
    }

    public class CustoRequestDto
    {
        [Required]
        public int LavouraId { get; set; }
        
        public DateTime? DataInicio { get; set; }
        
        public DateTime? DataFim { get; set; }
    }
}

