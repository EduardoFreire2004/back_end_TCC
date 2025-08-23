public class MovimentacaoEstoqueUpdateDTO
{
    public int id { get; set; }
    public int lavouraID { get; set; }
    public int movimentacao { get; set; }
    public int? agrotoxicoID { get; set; }
    public int? sementeID { get; set; }
    public int? insumoID { get; set; }
    public decimal qtde { get; set; }
    public DateTime dataHora { get; set; }
    public string descricao { get; set; }
    public int? origemAplicacaoID { get; set; }
    public int? origemAplicacaoInsumoID { get; set; }
    public int? origemPlantioID { get; set; }
}
