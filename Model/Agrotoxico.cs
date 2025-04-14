using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Agrotoxicos")]
    public class Agrotoxico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(35)]
        public FornecedorAgrotoxico fornecedor { get; set; }
        public int fornecedorID { get; set; }

        [Required]
        [StringLength(35)]
        public TipoAgrotoxico tipo { get; set; }
        public int tipoID { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [Required]
        public float unidade_Medida { get; set; }

        [Required]
        [StringLength(50)]
        public DateTime data_Cadastro { get; set; }

        [Required]
        public float qtde { get; set; }


    }
}
