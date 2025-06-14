﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TCC.Model
{
    [Table("Aplicacoes")]
    public class Aplicacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]       
        public int agrotoxicoID { get; set; }

        [Required]
        [StringLength(100)]
        public string descricao { get; set; }

        [Required]
        public DateTime dataHora { get; set; }

    }
}

