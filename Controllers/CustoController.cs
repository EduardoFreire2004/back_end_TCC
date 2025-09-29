using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API_TCC.Model;
using API_TCC.DTOs;

namespace API_TCC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustoController : ControllerBase
    {
        private readonly ICustoService _custoService;
        private readonly ILogger<CustoController> _logger;

        public CustoController(ICustoService custoService, ILogger<CustoController> logger)
        {
            _custoService = custoService;
            _logger = logger;
        }

        /// <summary>
        /// Calcula custos totais de uma lavoura
        /// </summary>
        [HttpPost("calcular/{lavouraId}")]
        public async Task<ActionResult<CustoCalculadoDto>> CalcularCustosLavoura(
            int lavouraId, 
            [FromBody] CustoRequestDto request)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var custos = await _custoService.CalcularCustosLavouraAsync(
                    lavouraId, 
                    usuarioId, 
                    request.DataInicio, 
                    request.DataFim);

                return Ok(custos);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular custos da lavoura {LavouraId}", lavouraId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Calcula custos de uma aplicação específica
        /// </summary>
        [HttpGet("aplicacao/{aplicacaoId}")]
        public async Task<ActionResult<CustoAplicacaoDto>> CalcularCustoAplicacao(int aplicacaoId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var custo = await _custoService.CalcularCustoAplicacaoAsync(aplicacaoId, usuarioId);
                return Ok(custo);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular custo da aplicação {AplicacaoId}", aplicacaoId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Calcula custos de uma aplicação de insumo específica
        /// </summary>
        [HttpGet("aplicacao-insumo/{aplicacaoInsumoId}")]
        public async Task<ActionResult<CustoAplicacaoInsumoDto>> CalcularCustoAplicacaoInsumo(int aplicacaoInsumoId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var custo = await _custoService.CalcularCustoAplicacaoInsumoAsync(aplicacaoInsumoId, usuarioId);
                return Ok(custo);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular custo da aplicação de insumo {AplicacaoInsumoId}", aplicacaoInsumoId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Calcula custos de uma movimentação específica
        /// </summary>
        [HttpGet("movimentacao/{movimentacaoId}")]
        public async Task<ActionResult<CustoMovimentacaoDto>> CalcularCustoMovimentacao(int movimentacaoId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var custo = await _custoService.CalcularCustoMovimentacaoAsync(movimentacaoId, usuarioId);
                return Ok(custo);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular custo da movimentação {MovimentacaoId}", movimentacaoId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obtém resumo de custos por período
        /// </summary>
        [HttpPost("resumo/{lavouraId}")]
        public async Task<ActionResult<ResumoCustosDto>> ObterResumoCustos(
            int lavouraId, 
            [FromBody] CustoRequestDto request)
        {
            try
            {
                if (!request.DataInicio.HasValue || !request.DataFim.HasValue)
                {
                    return BadRequest(new { message = "Data de início e fim são obrigatórias" });
                }

                var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var resumo = await _custoService.ObterResumoCustosAsync(
                    lavouraId, 
                    usuarioId, 
                    request.DataInicio.Value, 
                    request.DataFim.Value);

                return Ok(resumo);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo de custos da lavoura {LavouraId}", lavouraId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obtém histórico de custos de uma lavoura
        /// </summary>
        [HttpPost("historico/{lavouraId}")]
        public async Task<ActionResult<List<HistoricoCustoDto>>> ObterHistoricoCustos(
            int lavouraId, 
            [FromBody] CustoRequestDto request)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var historico = await _custoService.ObterHistoricoCustosAsync(
                    lavouraId, 
                    usuarioId, 
                    request.DataInicio, 
                    request.DataFim);

                return Ok(historico);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter histórico de custos da lavoura {LavouraId}", lavouraId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Atualiza custos de uma lavoura (recalcula tudo)
        /// </summary>
        [HttpPost("atualizar/{lavouraId}")]
        public async Task<ActionResult> AtualizarCustosLavoura(int lavouraId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var sucesso = await _custoService.AtualizarCustosLavouraAsync(lavouraId, usuarioId);
                
                if (sucesso)
                {
                    return Ok(new { message = "Custos atualizados com sucesso" });
                }
                else
                {
                    return StatusCode(500, new { message = "Erro ao atualizar custos" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar custos da lavoura {LavouraId}", lavouraId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }
    }
}












