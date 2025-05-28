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
        public int categoriaInsumoID { get; set; }

        [Required]
        public int fornecedorInsumoID { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [Required]
        public string unidade_Medida { get; set; }

        [Required]
        [StringLength(50)]
        public DateTime data_Cadastro { get; set; }

        [Required]
        public float qtde { get; set; }
    }
}
