namespace API_TCC.Model
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime DataExpiracao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        
        // Navegação
        public Usuario? Usuario { get; set; }
    }
}

