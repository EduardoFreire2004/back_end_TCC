using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class PlantioDto
    {
        [Required]
        public int lavouraID { get; set; }

        [Required]
        public int sementeID { get; set; }

        [Required]
        public DateTime data_Plantio { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double quantidade_Sementes { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double espacamento { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double profundidade { get; set; }

        public string? observacoes { get; set; }
    }

    public class PlantioResponseDto
    {
        public int Id { get; set; }
        public int lavouraID { get; set; }
        public int sementeID { get; set; }
        public DateTime data_Plantio { get; set; }
        public double quantidade_Sementes { get; set; }
        public double espacamento { get; set; }
        public double profundidade { get; set; }
        public string? observacoes { get; set; }
        public string LavouraNome { get; set; } = string.Empty;
        public string SementeNome { get; set; } = string.Empty;
    }
}

