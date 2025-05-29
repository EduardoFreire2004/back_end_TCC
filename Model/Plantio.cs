using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Plantios")]
    public class Plantio
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int sementeID { get; set; }

        [Required]
        [StringLength(50)]
        public string descricao { get; set; }

        [Required]
        public DateTime dataHora { get; set; }

        [Required]
        public float areaPlantada { get; set; }

    }
}
