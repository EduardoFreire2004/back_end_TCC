using Microsoft.EntityFrameworkCore;
using API_TCC.Model;

namespace API_TCC.Model
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly Contexto _context;

        public UsuarioRepository(Contexto context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Ativo);
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            // Definir valores padrão
            usuario.DataCadastro = DateTime.UtcNow;
            usuario.Ativo = true;
            
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            
            return usuario;
        }

        public async Task<Usuario> UpdateAsync(Usuario usuario)
        {
            var existingUsuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuario.Id && u.Ativo);
                
            if (existingUsuario == null)
                throw new InvalidOperationException("Usuário não encontrado");
            
            existingUsuario.Nome = usuario.Nome;
            existingUsuario.Email = usuario.Email;
            existingUsuario.Telefone = usuario.Telefone;
            
            await _context.SaveChangesAsync();
            return existingUsuario;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
                
            if (usuario == null)
                return false;
            
            usuario.Ativo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email == email && u.Ativo);
        }
    }
}









