using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API_TCC.Model;
using API_TCC.DTOs;

namespace API_TCC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<AuthResponseDto>> Cadastrar([FromBody] CadastroDto cadastroDto)
        {
            var result = await _authService.CadastrarAsync(cadastroDto);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshDto)
        {
            var result = await _authService.RefreshTokenAsync(refreshDto.refreshToken);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
                
            if (token != null)
            {
                await _authService.LogoutAsync(token);
            }
            
            return Ok(new { message = "Logout realizado com sucesso" });
        }

        [HttpGet("verificar")]
        [Authorize]
        public ActionResult Verificar()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            
            return Ok(new 
            { 
                message = "Token válido",
                userId = userId,
                userName = userName,
                userEmail = userEmail
            });
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<ActionResult<Usuario>> ObterPerfil()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return Unauthorized();
            }

            // Aqui você pode implementar a lógica para buscar o perfil completo do usuário
            // Por enquanto, retornamos apenas as informações básicas do token
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            
            var perfil = new Usuario
            {
                Id = id,
                Nome = userName ?? "",
                Email = userEmail ?? ""
            };
            
            return Ok(perfil);
        }
    }
}

