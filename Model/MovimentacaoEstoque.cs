using API_TCC.Enums;
using API_TCC.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("MovimentacoesEstoque")]
public class MovimentacaoEstoque
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public int lavouraID { get; set; }

    [ForeignKey(nameof(lavouraID))]
    public virtual Lavoura Lavoura { get; set; }

    [Required]
    public TipoMovimentacao movimentacao { get; set; }

    public int? agrotoxicoID { get; set; }
    [ForeignKey(nameof(agrotoxicoID))]
    public virtual Agrotoxico? Agrotoxico { get; set; }

    public int? sementeID { get; set; }
    [ForeignKey(nameof(sementeID))]
    public virtual Semente? Semente { get; set; }

    public int? insumoID { get; set; }
    [ForeignKey(nameof(insumoID))]
    public virtual Insumo? Insumo { get; set; }

    [Required]
    public float qtde { get; set; }

    [Required]
    public DateTime dataHora { get; set; }

    [StringLength(255)]
    public string? descricao { get; set; }

    public int? origemAplicacaoID { get; set; }
    public int? origemAplicacaoInsumoID { get; set; }
    public int? origemPlantioID { get; set; }

    // Navegação
    [ForeignKey("UsuarioId")]
    public virtual Usuario? Usuario { get; set; }
}