using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Fornecedores_Agrotoxicos")]
    public class FornecedorAgrotoxico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [Required]
        [StringLength(18)]
        public string cnpj { get; set; }

        [Required]
        [StringLength(18)]
        public string telefone { get; set; }


    }
}
