using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Colheitas")]
    public class Colheita
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string tipo { get; set; }

        [Required]
        public DateTime dataHora { get; set; }

        [StringLength(100)]
        public string descricao { get; set; }
    }
}
 