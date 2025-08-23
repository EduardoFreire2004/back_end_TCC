using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Custos")]
    public class Custo
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

        public int? aplicacaoAgrotoxicoID { get; set; }

        [ForeignKey(nameof(aplicacaoAgrotoxicoID))]
        public virtual Aplicacao aplicacao { get; set; }

        public int? aplicacaoInsumoID { get; set; }

        [ForeignKey(nameof(aplicacaoInsumoID))]
        public virtual AplicacaoInsumos aplicacaoInsumo { get; set; }

        public int? plantioID { get; set; }

        [ForeignKey(nameof(plantioID))]
        public virtual Plantio plantio { get; set; }

        public int? colheitaID { get; set; }

        [ForeignKey(nameof(colheitaID))]
        public virtual Colheita colheita { get; set; }

        public double custoTotal { get; set; }

        public double ganhoTotal { get; set; }

        // Navegação
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}
