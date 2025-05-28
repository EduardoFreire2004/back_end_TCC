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
        public int fornecedorID { get; set; }

        [Required]
        public int tipoID { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [Required]
        public string unidade_Medida { get; set; }

        [Required]
        public DateTime data_Cadastro { get; set; }

        [Required]
        public float qtde { get; set; }


    }
}
