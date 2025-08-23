using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        public string senha { get; set; } = string.Empty;
    }

    public class CadastroDto
    {
        [Required]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string senha { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string telefone { get; set; } = string.Empty;
    }

    public class RefreshTokenDto
    {
        [Required]
        public string refreshToken { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string? Message { get; set; }
        public UsuarioResponseDto Usuario { get; set; } = new();
        public DateTime ExpiresAt { get; set; }
    }

    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string telefone { get; set; } = string.Empty;
    }
}
