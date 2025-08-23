using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace API_TCC.Model
{
    public class EstoqueService : IEstoqueService
    {
        private readonly string _connectionString;

        public EstoqueService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("conexao")!;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<bool> VerificarEstoqueSementeAsync(int sementeId, float quantidade, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT qtde FROM Sementes 
                WHERE Id = @SementeId AND UsuarioId = @UsuarioId AND qtde >= @Quantidade";
                
            var estoque = await connection.QueryFirstOrDefaultAsync<float?>(sql, new { SementeId = sementeId, UsuarioId = usuarioId, Quantidade = quantidade });
            return estoque.HasValue;
        }

        public async Task<bool> VerificarEstoqueAgrotoxicoAsync(int agrotoxicoId, float quantidade, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT qtde FROM Agrotoxicos 
                WHERE Id = @AgrotoxicoId AND UsuarioId = @UsuarioId AND qtde >= @Quantidade";
                
            var estoque = await connection.QueryFirstOrDefaultAsync<float?>(sql, new { AgrotoxicoId = agrotoxicoId, UsuarioId = usuarioId, Quantidade = quantidade });
            return estoque.HasValue;
        }

        public async Task<bool> VerificarEstoqueInsumoAsync(int insumoId, float quantidade, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT qtde FROM Insumos 
                WHERE Id = @InsumoId AND UsuarioId = @UsuarioId AND qtde >= @Quantidade";
                
            var estoque = await connection.QueryFirstOrDefaultAsync<float?>(sql, new { InsumoId = insumoId, UsuarioId = usuarioId, Quantidade = quantidade });
            return estoque.HasValue;
        }

        public async Task<bool> BaixarEstoqueSementeAsync(int sementeId, float quantidade, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE Sementes 
                SET qtde = qtde - @Quantidade 
                WHERE Id = @SementeId AND UsuarioId = @UsuarioId AND qtde >= @Quantidade";
                
            var rowsAffected = await connection.ExecuteAsync(sql, new { SementeId = sementeId, UsuarioId = usuarioId, Quantidade = quantidade });
            return rowsAffected > 0;
        }

        public async Task<bool> BaixarEstoqueAgrotoxicoAsync(int agrotoxicoId, float quantidade, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE Agrotoxicos 
                SET qtde = qtde - @Quantidade 
                WHERE Id = @AgrotoxicoId AND UsuarioId = @UsuarioId AND qtde >= @Quantidade";
                
            var rowsAffected = await connection.ExecuteAsync(sql, new { AgrotoxicoId = agrotoxicoId, UsuarioId = usuarioId, Quantidade = quantidade });
            return rowsAffected > 0;
        }

        public async Task<bool> BaixarEstoqueInsumoAsync(int insumoId, float quantidade, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE Insumos 
                SET qtde = qtde - @Quantidade 
                WHERE Id = @InsumoId AND UsuarioId = @UsuarioId AND qtde >= @Quantidade";
                
            var rowsAffected = await connection.ExecuteAsync(sql, new { InsumoId = insumoId, UsuarioId = usuarioId, Quantidade = quantidade });
            return rowsAffected > 0;
        }

        public async Task<bool> RetornarEstoqueSementeAsync(int sementeId, float quantidade, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE Sementes 
                SET qtde = qtde + @Quantidade 
                WHERE Id = @SementeId AND UsuarioId = @UsuarioId";
                
            var rowsAffected = await connection.ExecuteAsync(sql, new { SementeId = sementeId, UsuarioId = usuarioId, Quantidade = quantidade });
            return rowsAffected > 0;
        }

        public async Task<bool> RetornarEstoqueAgrotoxicoAsync(int agrotoxicoId, float quantidade, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE Agrotoxicos 
                SET qtde = qtde + @Quantidade 
                WHERE Id = @AgrotoxicoId AND UsuarioId = @UsuarioId";
                
            var rowsAffected = await connection.ExecuteAsync(sql, new { AgrotoxicoId = agrotoxicoId, UsuarioId = usuarioId, Quantidade = quantidade });
            return rowsAffected > 0;
        }

        public async Task<bool> RetornarEstoqueInsumoAsync(int insumoId, float quantidade, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE Insumos 
                SET qtde = qtde + @Quantidade 
                WHERE Id = @InsumoId AND UsuarioId = @UsuarioId";
                
            var rowsAffected = await connection.ExecuteAsync(sql, new { InsumoId = insumoId, UsuarioId = usuarioId, Quantidade = quantidade });
            return rowsAffected > 0;
        }

        public async Task<float> ObterEstoqueDisponivelSementeAsync(int sementeId, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT qtde FROM Sementes 
                WHERE Id = @SementeId AND UsuarioId = @UsuarioId";
                
            var estoque = await connection.QueryFirstOrDefaultAsync<float?>(sql, new { SementeId = sementeId, UsuarioId = usuarioId });
            return estoque ?? 0;
        }

        public async Task<float> ObterEstoqueDisponivelAgrotoxicoAsync(int agrotoxicoId, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT qtde FROM Agrotoxicos 
                WHERE Id = @AgrotoxicoId AND UsuarioId = @UsuarioId";
                
            var estoque = await connection.QueryFirstOrDefaultAsync<float?>(sql, new { AgrotoxicoId = agrotoxicoId, UsuarioId = usuarioId });
            return estoque ?? 0;
        }

        public async Task<float> ObterEstoqueDisponivelInsumoAsync(int insumoId, int usuarioId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT qtde FROM Insumos 
                WHERE Id = @InsumoId AND UsuarioId = @UsuarioId";
                
            var estoque = await connection.QueryFirstOrDefaultAsync<float?>(sql, new { InsumoId = insumoId, UsuarioId = usuarioId });
            return estoque ?? 0;
        }
    }
}



