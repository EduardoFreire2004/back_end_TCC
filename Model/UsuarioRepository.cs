using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_TCC.Model
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("conexao")!;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Usuarios WHERE Id = @Id AND Ativo = 1";
            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Usuarios WHERE Email = @Email AND Ativo = 1";
            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                INSERT INTO Usuarios (Nome, Email, Senha, Telefone, DataCadastro, Ativo)
                OUTPUT INSERTED.*
                VALUES (@Nome, @Email, @Senha, @Telefone, @DataCadastro, @Ativo)";
            
            return await connection.QueryFirstAsync<Usuario>(sql, usuario);
        }

        public async Task<Usuario> UpdateAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE Usuarios 
                SET Nome = @Nome, Email = @Email, Telefone = @Telefone
                OUTPUT INSERTED.*
                WHERE Id = @Id AND Ativo = 1";
            
            return await connection.QueryFirstAsync<Usuario>(sql, usuario);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Usuarios SET Ativo = 0 WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM Usuarios WHERE Email = @Email AND Ativo = 1";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
            return count > 0;
        }
    }
}








