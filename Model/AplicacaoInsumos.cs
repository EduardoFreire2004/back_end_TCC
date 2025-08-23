using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Aplicacao_Insumos")]
    public class AplicacaoInsumos
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
        public int insumoID { get; set; }

        [ForeignKey(nameof(insumoID))]
        public virtual Insumo insumo { get; set; }

        [StringLength(100)]
        public string descricao { get; set; }

        [Required]
        public DateTime dataHora { get; set; }

        [Required]
        public float qtde { get; set; }

        // Navegação
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}

