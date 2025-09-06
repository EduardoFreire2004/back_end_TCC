using Microsoft.EntityFrameworkCore;
using API_TCC.Model;

namespace API_TCC.Model
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly Contexto _context;

        public RefreshTokenRepository(Contexto context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token && 
                                          rt.Ativo && 
                                          rt.DataExpiracao > DateTime.UtcNow);
        }

        public async Task<RefreshToken> SaveAsync(int usuarioId, string token, DateTime dataExpiracao)
        {
            // Primeiro, invalidar tokens anteriores do usuário
            await InvalidateByUsuarioAsync(usuarioId);
            
            var refreshToken = new RefreshToken
            {
                UsuarioId = usuarioId,
                Token = token,
                DataExpiracao = dataExpiracao,
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            };
            
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            
            return refreshToken;
        }

        public async Task<bool> InvalidateAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
                
            if (refreshToken == null)
                return false;
            
            refreshToken.Ativo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InvalidateByUsuarioAsync(int usuarioId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UsuarioId == usuarioId && rt.Ativo)
                .ToListAsync();
                
            if (!tokens.Any())
                return false;
            
            foreach (var token in tokens)
            {
                token.Ativo = false;
            }
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.DataExpiracao <= DateTime.UtcNow && rt.Ativo)
                .ToListAsync();
                
            foreach (var token in expiredTokens)
            {
                token.Ativo = false;
            }
            
            await _context.SaveChangesAsync();
        }
    }
}
