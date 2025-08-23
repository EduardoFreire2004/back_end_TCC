using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class CategoriaInsumoDto
    {
        [Required]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

        [Required]
        public string descricao { get; set; } = string.Empty;
    }

    public class CategoriaInsumoResponseDto
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string descricao { get; set; } = string.Empty;
    }

    public class TipoAgrotoxicoDto
    {
        [Required]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

        [Required]
        public string descricao { get; set; } = string.Empty;

        [Required]
        public string classe_Toxicologica { get; set; } = string.Empty;
    }

    public class TipoAgrotoxicoResponseDto
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string descricao { get; set; } = string.Empty;
        public string classe_Toxicologica { get; set; } = string.Empty;
    }
}

