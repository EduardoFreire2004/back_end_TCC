namespace API_TCC.DTOs
{
    public class ColheitaDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int LavouraID { get; set; }

        // Apenas o nome da lavoura (para exibição simples)
        public string? NomeLavoura { get; set; }

        public string Tipo { get; set; }
        public DateTime DataHora { get; set; }
        public string? Descricao { get; set; }
        public double QuantidadeSacas { get; set; }
        public double AreaHectares { get; set; }
        public string CooperativaDestino { get; set; }
        public double PrecoPorSaca { get; set; }

    }
}