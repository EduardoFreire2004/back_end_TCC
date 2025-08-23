using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Sementes")]
    public class Semente
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int fornecedorID { get; set; }

        [ForeignKey(nameof(fornecedorID))]
        public virtual Fornecedor fornecedor { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [Required]
        [StringLength(50)]
        public string tipo { get; set; }

        [Required]
        [StringLength(50)]
        public string marca { get; set; }

        [Required]
        public float qtde { get; set; }

        [Required]
        public DateTime data_Cadastro { get; set; }

        [Required]
        public float preco { get; set; }

        // Navegação
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}
