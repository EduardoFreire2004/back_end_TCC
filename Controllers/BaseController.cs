using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_TCC.Controllers
{
    [ApiController]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        protected int GetUsuarioId()
        {
            // Primeiro tenta buscar pelo claim específico UsuarioId
            var userId = User.FindFirst("UsuarioId")?.Value;
            
            // Se não encontrar, tenta pelo NameIdentifier (fallback)
            if (string.IsNullOrEmpty(userId))
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado");
            }
            return id;
        }

        protected string GetUsuarioNome()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

        protected string GetUsuarioEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
    }
}

