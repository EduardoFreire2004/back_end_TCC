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

        public int? insumoID { get; set; }

        public int? aplicacaoID { get; set; }

        public int? plantioID { get; set; }

        public int? colheitaID { get; set; }

        [Required]
        public float area { get; set; }

        [Required]
        public string nome { get; set; }

    }
}
