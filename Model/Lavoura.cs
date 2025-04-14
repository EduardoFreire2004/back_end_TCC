using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Lavouras")]
    public class Lavoura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(35)]
        public Insumo insumo { get; set; }
        public int? insumoID { get; set; }

        [Required]
        [StringLength(35)]
        public Aplicacao aplicacao { get; set; }
        public int? aplicacaoID { get; set; }

        [Required]
        [StringLength(35)]
        public Plantio  plantio { get; set; }
        public int? plantioID { get; set; }

        [Required]
        [StringLength(35)]
        public Colheita colheita { get; set; }
        public int? colheitaID { get; set; }

        [Required]
        public float area { get; set; }

        [Required]
        public string nome { get; set; }

    }
}
