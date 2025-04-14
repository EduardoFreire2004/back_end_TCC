using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_TCC.Model
{
    [Table("Fornecedores_Sementes")]
    public class FornecedorSemente
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
