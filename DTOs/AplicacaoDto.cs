using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class AplicacaoDto
    {
        [Required]
        public int lavouraID { get; set; }

        [Required]
        public DateTime data_Aplicacao { get; set; }

        [Required]
        [StringLength(100)]
        public string tipo_Aplicacao { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double quantidade_Aplicada { get; set; }

        [Required]
        public string unidade_Medida { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double area_Aplicada { get; set; }

        public string? observacoes { get; set; }

        [Required]
        public string condicoes_Climaticas { get; set; } = string.Empty;
    }

    public class AplicacaoResponseDto
    {
        public int Id { get; set; }
        public int lavouraID { get; set; }
        public DateTime data_Aplicacao { get; set; }
        public string tipo_Aplicacao { get; set; } = string.Empty;
        public double quantidade_Aplicada { get; set; }
        public string unidade_Medida { get; set; } = string.Empty;
        public double area_Aplicada { get; set; }
        public string? observacoes { get; set; }
        public string condicoes_Climaticas { get; set; } = string.Empty;
        public string LavouraNome { get; set; } = string.Empty;
    }
}

