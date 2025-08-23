using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class ColheitaDto
    {
        [Required]
        public int lavouraID { get; set; }

        [Required]
        public DateTime data_Colheita { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double quantidade_Colhida { get; set; }

        [Required]
        public string unidade_Medida { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double area_Colhida { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double produtividade { get; set; }

        public string? observacoes { get; set; }

        [Required]
        public string qualidade { get; set; } = string.Empty;
    }

    public class ColheitaResponseDto
    {
        public int Id { get; set; }
        public int lavouraID { get; set; }
        public DateTime data_Colheita { get; set; }
        public double quantidade_Colhida { get; set; }
        public string unidade_Medida { get; set; } = string.Empty;
        public double area_Colhida { get; set; }
        public double produtividade { get; set; }
        public string? observacoes { get; set; }
        public string qualidade { get; set; } = string.Empty;
        public string LavouraNome { get; set; } = string.Empty;
        public string Cultura { get; set; } = string.Empty;
    }
}

