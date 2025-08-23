using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Insumos")]
    public class Insumo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int categoriaInsumoID { get; set; }

        [ForeignKey(nameof(categoriaInsumoID))]
        public virtual CategoriaInsumo categoriaInsumo { get; set; }

        [Required]
        public int fornecedorID { get; set; }

        [ForeignKey(nameof(fornecedorID))]
        public virtual Fornecedor fornecedor { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [Required]
        public string unidade_Medida { get; set; }

        [Required]
        public DateTime data_Cadastro { get; set; }

        [Required]
        public float qtde { get; set; }

        [Required]
        public float preco { get; set; }

        // Navegação
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}
