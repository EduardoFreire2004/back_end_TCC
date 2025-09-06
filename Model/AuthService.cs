using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API_TCC.DTOs;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration configuration, 
                          IUsuarioRepository usuarioRepository,
                          IRefreshTokenRepository refreshTokenRepository,
                          ILogger<AuthService> logger)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Tentativa de login para o email: {Email}", loginDto.email);
                
                var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.email);
                if (usuario == null || !VerifyPassword(loginDto.senha, usuario.Senha))
                {
                    _logger.LogWarning("Login falhou para o email: {Email}", loginDto.email);
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

                _logger.LogInformation("Login bem-sucedido para o usuário: {UsuarioId}", usuario.Id);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o login para o email: {Email}", loginDto.email);
                throw;
            }
        }

        public async Task<AuthResponseDto> CadastrarAsync(CadastroDto cadastroDto)
        {
            try
            {
                _logger.LogInformation("Iniciando cadastro para o email: {Email}", cadastroDto.email);
                
                // Verificar se o email já existe
                var existingUsuario = await _usuarioRepository.GetByEmailAsync(cadastroDto.email);
                if (existingUsuario != null)
                {
                    _logger.LogWarning("Tentativa de cadastro com email já existente: {Email}", cadastroDto.email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email já cadastrado"
                    };
                }

                _logger.LogInformation("Criando novo usuário para o email: {Email}", cadastroDto.email);
                
                var usuario = new Usuario
                {
                    Nome = cadastroDto.nome,
                    Email = cadastroDto.email,
                    Senha = HashPassword(cadastroDto.senha),
                    Telefone = cadastroDto.telefone
                };

                _logger.LogInformation("Salvando usuário no banco de dados...");
                await _usuarioRepository.CreateAsync(usuario);
                _logger.LogInformation("Usuário criado com sucesso. ID: {UsuarioId}", usuario.Id);

                var token = GenerateJwtToken(usuario);
                var refreshToken = GenerateRefreshToken();
                var expiresAt = DateTime.UtcNow.AddHours(24);

                _logger.LogInformation("Gerando refresh token para o usuário: {UsuarioId}", usuario.Id);
                // Salvar o refresh token
                await _refreshTokenRepository.SaveAsync(usuario.Id, refreshToken, expiresAt);

                _logger.LogInformation("Cadastro concluído com sucesso para o usuário: {UsuarioId}", usuario.Id);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o cadastro para o email: {Email}", cadastroDto.email);
                throw;
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                _logger.LogInformation("Tentativa de refresh token");
                
                var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
                if (tokenEntity == null || tokenEntity.DataExpiracao < DateTime.UtcNow)
                {
                    _logger.LogWarning("Refresh token inválido ou expirado");
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Token de renovação inválido ou expirado"
                    };
                }

                var usuario = await _usuarioRepository.GetByIdAsync(tokenEntity.UsuarioId);
                if (usuario == null)
                {
                    _logger.LogWarning("Usuário não encontrado para refresh token");
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

                _logger.LogInformation("Refresh token realizado com sucesso para o usuário: {UsuarioId}", usuario.Id);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o refresh token");
                throw;
            }
        }

        public async Task<bool> LogoutAsync(string token)
        {
            try
            {
                _logger.LogInformation("Tentativa de logout");
                
                // Invalidar refresh token
                var result = await _refreshTokenRepository.InvalidateAsync(token);
                
                if (result)
                {
                    _logger.LogInformation("Logout realizado com sucesso");
                }
                else
                {
                    _logger.LogWarning("Logout falhou - token não encontrado");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o logout");
                return false;
            }
        }

        public async Task<bool> VerificarTokenAsync(string token)
        {
            try
            {
                _logger.LogInformation("Verificando token JWT");
                
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

                _logger.LogInformation("Token JWT válido");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token JWT inválido");
                return false;
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            try
            {
                _logger.LogInformation("Gerando JWT token para o usuário: {UsuarioId}", usuario.Id);
                
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
                var tokenString = tokenHandler.WriteToken(token);
                
                _logger.LogInformation("JWT token gerado com sucesso para o usuário: {UsuarioId}", usuario.Id);
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar JWT token para o usuário: {UsuarioId}", usuario.Id);
                throw;
            }
        }

        private string GenerateRefreshToken()
        {
            try
            {
                _logger.LogInformation("Gerando refresh token");
                
                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                var token = Convert.ToBase64String(randomNumber);
                
                _logger.LogInformation("Refresh token gerado com sucesso");
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar refresh token");
                throw;
            }
        }

        private string HashPassword(string password)
        {
            try
            {
                _logger.LogInformation("Hashando senha");
                
                using var sha256 = SHA256.Create();
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedPassword = Convert.ToBase64String(hashedBytes);
                
                _logger.LogInformation("Senha hashada com sucesso");
                return hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao hashar senha");
                throw;
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                _logger.LogInformation("Verificando senha");
                
                var hashedInput = HashPassword(password);
                var isValid = hashedInput == hashedPassword;
                
                _logger.LogInformation("Verificação de senha concluída: {IsValid}", isValid);
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar senha");
                return false;
            }
        }
    }
}

