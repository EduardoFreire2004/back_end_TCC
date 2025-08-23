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

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int lavouraID { get; set; }

        [ForeignKey(nameof(lavouraID))]
        public virtual Lavoura lavoura { get; set; }

        [Required]
        public int sementeID { get; set; }

        [ForeignKey(nameof(sementeID))]
        public virtual Semente semente { get; set; }

        [Required]
        [StringLength(50)]
        public string descricao { get; set; }

        [Required]
        public DateTime dataHora { get; set; }

        [Required]
        public float areaPlantada { get; set; }

        [Required]
        public float qtde { get; set; }

        // Navegação
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}
