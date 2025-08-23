using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class SementeDto
    {
        [Required]
        public int fornecedorSementeID { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

        [Required]
        public string especie { get; set; } = string.Empty;

        [Required]
        public string variedade { get; set; } = string.Empty;

        [Required]
        public string unidade_Medida { get; set; } = string.Empty;

        [Required]
        public DateTime data_Cadastro { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float qtde { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float preco { get; set; }

        public string? observacoes { get; set; }
    }

    public class SementeResponseDto
    {
        public int Id { get; set; }
        public int fornecedorSementeID { get; set; }
        public string nome { get; set; } = string.Empty;
        public string especie { get; set; } = string.Empty;
        public string variedade { get; set; } = string.Empty;
        public string unidade_Medida { get; set; } = string.Empty;
        public DateTime data_Cadastro { get; set; }
        public float qtde { get; set; }
        public float preco { get; set; }
        public string? observacoes { get; set; }
        public string FornecedorNome { get; set; } = string.Empty;
    }
}

