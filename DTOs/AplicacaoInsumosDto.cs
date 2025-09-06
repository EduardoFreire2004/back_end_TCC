using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class AplicacaoInsumosCreateDto
    {
        [Required]
        public int lavouraID { get; set; }

        [Required]
        public int insumoID { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public float qtde { get; set; }

        [Required]
        public DateTime dataHora { get; set; }

        [StringLength(100)]
        public string? descricao { get; set; }
    }

    public class AplicacaoInsumosUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int lavouraID { get; set; }

        [Required]
        public int insumoID { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public float qtde { get; set; }

        [Required]
        public DateTime dataHora { get; set; }

        [StringLength(100)]
        public string? descricao { get; set; }
    }

    public class AplicacaoInsumosResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int lavouraID { get; set; }
        public int insumoID { get; set; }
        public float qtde { get; set; }
        public DateTime dataHora { get; set; }
        public string? descricao { get; set; }
        
        // Campos de navegação
        public string LavouraNome { get; set; } = string.Empty;
        public string InsumoNome { get; set; } = string.Empty;
        public string InsumoUnidadeMedida { get; set; } = string.Empty;
    }
}

