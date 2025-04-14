using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Movimentacoes")]
    public class Movimentacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(35)]
        public Lavoura lavoura { get; set; }
        public int lavouraID { get; set; }

        [Required]
        [StringLength(35)]
        public TipoMovimentacao movimentacao { get; set; }

        [Required]
        public float qtde { get; set; }

        [Required]
        public DateTime dataHora { get; set; }
    }
}
