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
        public int UsuarioId { get; set; }

        [Required]
        public float area { get; set; }

        [Required]
        public string nome { get; set; }

        // Navegação
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }

        // Coleção de custos relacionados a esta lavoura
        public virtual ICollection<Custo> Custos { get; set; } = new List<Custo>();
    }
}