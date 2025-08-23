using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class CustoDto
    {
        [Required]
        public int lavouraID { get; set; }

        [Required]
        [StringLength(100)]
        public string descricao { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double valor { get; set; }

        [Required]
        public DateTime data_Custo { get; set; }

        [Required]
        [StringLength(50)]
        public string categoria { get; set; } = string.Empty;

        [Required]
        public string tipo_Custo { get; set; } = string.Empty;

        public string? observacoes { get; set; }
    }

    public class CustoResponseDto
    {
        public int Id { get; set; }
        public int lavouraID { get; set; }
        public string descricao { get; set; } = string.Empty;
        public double valor { get; set; }
        public DateTime data_Custo { get; set; }
        public string categoria { get; set; } = string.Empty;
        public string tipo_Custo { get; set; } = string.Empty;
        public string? observacoes { get; set; }
        public string LavouraNome { get; set; } = string.Empty;
        public string Cultura { get; set; } = string.Empty;
    }
}

