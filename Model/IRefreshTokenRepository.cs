using API_TCC.Model;

namespace API_TCC.Model
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<RefreshToken> SaveAsync(int usuarioId, string token, DateTime dataExpiracao);
        Task<bool> InvalidateAsync(string token);
        Task<bool> InvalidateByUsuarioAsync(int usuarioId);
        Task CleanupExpiredTokensAsync();
    }
}

