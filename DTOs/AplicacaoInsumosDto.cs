using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class AplicacaoInsumosDto
    {
        [Required]
        public int aplicacaoID { get; set; }

        [Required]
        public int insumoID { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double quantidade_Utilizada { get; set; }

        [Required]
        public string unidade_Medida { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double custo_Unitario { get; set; }

        public string? observacoes { get; set; }
    }

    public class AplicacaoInsumosResponseDto
    {
        public int Id { get; set; }
        public int aplicacaoID { get; set; }
        public int insumoID { get; set; }
        public double quantidade_Utilizada { get; set; }
        public string unidade_Medida { get; set; } = string.Empty;
        public double custo_Unitario { get; set; }
        public string? observacoes { get; set; }
        public string InsumoNome { get; set; } = string.Empty;
        public string AplicacaoTipo { get; set; } = string.Empty;
    }
}

