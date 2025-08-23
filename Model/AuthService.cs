using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API_TCC.DTOs;

namespace API_TCC.Model
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> CadastrarAsync(CadastroDto cadastroDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(string token);
        Task<bool> VerificarTokenAsync(string token);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IConfiguration configuration, 
                          IUsuarioRepository usuarioRepository,
                          IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.email);
            if (usuario == null || !VerifyPassword(loginDto.senha, usuario.Senha))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email ou senha inválidos"
                };
            }

            var token = GenerateJwtToken(usuario);
            var refreshToken = GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddHours(24);

            // Salvar o refresh token
            await _refreshTokenRepository.SaveAsync(usuario.Id, refreshToken, expiresAt);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                Usuario = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    telefone = usuario.Telefone ?? string.Empty
                },
                ExpiresAt = expiresAt
            };
        }

        public async Task<AuthResponseDto> CadastrarAsync(CadastroDto cadastroDto)
        {
            if (await _usuarioRepository.GetByEmailAsync(cadastroDto.email) != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email já cadastrado"
                };
            }

            var usuario = new Usuario
            {
                Nome = cadastroDto.nome,
                Email = cadastroDto.email,
                Senha = HashPassword(cadastroDto.senha),
                Telefone = cadastroDto.telefone
            };

            await _usuarioRepository.CreateAsync(usuario);

            var token = GenerateJwtToken(usuario);
            var refreshToken = GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddHours(24);

            // Salvar o refresh token
            await _refreshTokenRepository.SaveAsync(usuario.Id, refreshToken, expiresAt);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                Usuario = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    telefone = usuario.Telefone ?? string.Empty
                },
                ExpiresAt = expiresAt
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (tokenEntity == null || tokenEntity.DataExpiracao < DateTime.UtcNow)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Token de renovação inválido ou expirado"
                };
            }

            var usuario = await _usuarioRepository.GetByIdAsync(tokenEntity.UsuarioId);
            if (usuario == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Usuário não encontrado"
                };
            }

            var newToken = GenerateJwtToken(usuario);
            var newRefreshToken = GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddHours(24);

            // Salvar o novo refresh token
            await _refreshTokenRepository.SaveAsync(usuario.Id, newRefreshToken, expiresAt);

            return new AuthResponseDto
            {
                Success = true,
                Token = newToken,
                RefreshToken = newRefreshToken,
                Usuario = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    telefone = usuario.Telefone ?? string.Empty
                },
                ExpiresAt = expiresAt
            };
        }

        public async Task<bool> LogoutAsync(string token)
        {
            try
            {
                // Invalidar refresh token
                await _refreshTokenRepository.InvalidateAsync(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> VerificarTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "");
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim("UsuarioId", usuario.Id.ToString()), // Claim específico para UsuarioId
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60")
                ),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }
}

