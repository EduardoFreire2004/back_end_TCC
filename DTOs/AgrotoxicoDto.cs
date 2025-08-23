using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class AgrotoxicoDto
    {
        [Required]
        public int fornecedorID { get; set; }

        [Required]
        public int tipoID { get; set; }

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
    }

    public class AgrotoxicoResponseDto
    {
        public int Id { get; set; }
        public int fornecedorID { get; set; }
        public int tipoID { get; set; }
        public string nome { get; set; } = string.Empty;
        public string unidade_Medida { get; set; } = string.Empty;
        public DateTime data_Cadastro { get; set; }
        public float qtde { get; set; }
        public float preco { get; set; }
        public string FornecedorNome { get; set; } = string.Empty;
        public string TipoNome { get; set; } = string.Empty;
    }
}
