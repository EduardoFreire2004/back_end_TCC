using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace API_TCC.Model
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly string _connectionString;

        public RefreshTokenRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("conexao")!;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT Id, UsuarioId, Token, DataExpiracao, Ativo, DataCriacao
                FROM RefreshTokens 
                WHERE Token = @Token AND Ativo = 1 AND DataExpiracao > GETDATE()";
                
            return await connection.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Token = token });
        }

        public async Task<RefreshToken> SaveAsync(int usuarioId, string token, DateTime dataExpiracao)
        {
            using var connection = CreateConnection();
            
            // Primeiro, invalidar tokens anteriores do usuário
            await InvalidateByUsuarioAsync(usuarioId);
            
            const string sql = @"
                INSERT INTO RefreshTokens (UsuarioId, Token, DataExpiracao, Ativo, DataCriacao)
                OUTPUT INSERTED.*
                VALUES (@UsuarioId, @Token, @DataExpiracao, 1, GETDATE())";
                
            var refreshToken = new
            {
                UsuarioId = usuarioId,
                Token = token,
                DataExpiracao = dataExpiracao
            };
                
            return await connection.QueryFirstAsync<RefreshToken>(sql, refreshToken);
        }

        public async Task<bool> InvalidateAsync(string token)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE RefreshTokens 
                SET Ativo = 0 
                WHERE Token = @Token";
                
            var rowsAffected = await connection.ExecuteAsync(sql, new { Token = token });
            return rowsAffected > 0;
        }

        public async Task<bool> InvalidateByUsuarioAsync(int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE RefreshTokens 
                SET Ativo = 0 
                WHERE UsuarioId = @UsuarioId";
                
            var rowsAffected = await connection.ExecuteAsync(sql, new { UsuarioId = usuarioId });
            return rowsAffected > 0;
        }

        public async Task CleanupExpiredTokensAsync()
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE RefreshTokens 
                SET Ativo = 0 
                WHERE DataExpiracao <= GETDATE()";
                
            await connection.ExecuteAsync(sql);
        }
    }
}
