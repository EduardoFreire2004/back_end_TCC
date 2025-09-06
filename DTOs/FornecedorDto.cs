using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class FornecedorDto
    {
        [Required]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

        [Required]
        [StringLength(18)]
        public string cnpj { get; set; } = string.Empty;

        [Required]
        [StringLength(18)]
        public string telefone { get; set; } = string.Empty;
    }

    public class FornecedorResponseDto
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string cnpj { get; set; } = string.Empty;
        public string telefone { get; set; } = string.Empty;
    }
}

