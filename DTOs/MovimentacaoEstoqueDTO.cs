using System;
using System.ComponentModel.DataAnnotations;
using API_TCC.Enums;

namespace API_TCC.DTOs
{
    public class MovimentacaoEstoqueCreateDTO
    {
        [Required]
        public int lavouraID { get; set; }

        [Required]
        public TipoMovimentacao movimentacao { get; set; }

        public int? agrotoxicoID { get; set; }
        public int? sementeID { get; set; }
        public int? insumoID { get; set; }

        [Required]
        public float qtde { get; set; }

        [Required]
        public DateTime dataHora { get; set; }

        public string? descricao { get; set; }
    }
}
