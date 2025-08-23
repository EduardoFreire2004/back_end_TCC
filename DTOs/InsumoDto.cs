using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class InsumoDto
    {
        [Required]
        public int categoriaInsumoID { get; set; }

        [Required]
        public int fornecedorInsumoID { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

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

        public string? descricao { get; set; }
    }

    public class InsumoResponseDto
    {
        public int Id { get; set; }
        public int categoriaInsumoID { get; set; }
        public int fornecedorInsumoID { get; set; }
        public string nome { get; set; } = string.Empty;
        public string unidade_Medida { get; set; } = string.Empty;
        public DateTime data_Cadastro { get; set; }
        public float qtde { get; set; }
        public float preco { get; set; }
        public string? descricao { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
        public string FornecedorNome { get; set; } = string.Empty;
    }
}

