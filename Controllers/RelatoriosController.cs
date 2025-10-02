using Microsoft.AspNetCore.Mvc;
using API_TCC.DTOs;
using API_TCC.Model;

namespace API_TCC.Controllers
{
    [Route("api/relatorios")]
    [ApiController]
    public class RelatoriosController : BaseController
    {
        private readonly IRelatorioJsonService _relatorioService;

        public RelatoriosController(IRelatorioJsonService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        /// <summary>
        /// Relatório de fornecedores
        /// </summary>
        [HttpGet("fornecedores/{lavouraId}")]
        public async Task<IActionResult> RelatorioFornecedores(int lavouraId)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _relatorioService.GerarRelatorioFornecedoresAsync(usuarioId, lavouraId);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpGet("aplicacao/{lavouraId}")]
        public async Task<IActionResult> RelatorioAplicacao(int lavouraId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _relatorioService.GerarRelatorioAplicacaoAsync(usuarioId, lavouraId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpGet("aplicacao-insumo/{lavouraId}")]
        public async Task<IActionResult> RelatorioAplicacaoInsumo(int lavouraId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _relatorioService.GerarRelatorioAplicacaoInsumoAsync(usuarioId, lavouraId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpGet("semente/{lavouraId}")]
        public async Task<IActionResult> RelatorioSemente(int lavouraId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _relatorioService.GerarRelatorioSementeAsync(usuarioId, lavouraId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpGet("insumo/{lavouraId}")]
        public async Task<IActionResult> RelatorioInsumo(int lavouraId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _relatorioService.GerarRelatorioInsumoAsync(usuarioId, lavouraId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpGet("agrotoxico/{lavouraId}")]
        public async Task<IActionResult> RelatorioAgrotoxico(int lavouraId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _relatorioService.GerarRelatorioAgrotoxicoAsync(usuarioId, lavouraId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpGet("colheita/{lavouraId}")]
        public async Task<IActionResult> RelatorioColheita(int lavouraId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _relatorioService.GerarRelatorioColheitaAsync(usuarioId, lavouraId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpGet("movimentacao-estoque/{lavouraId}")]
        public async Task<IActionResult> RelatorioMovimentacaoEstoque(int lavouraId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _relatorioService.GerarRelatorioMovimentacaoEstoqueAsync(usuarioId, lavouraId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpGet("plantio/{lavouraId}")]
        public async Task<IActionResult> RelatorioPlantio(int lavouraId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _relatorioService.GerarRelatorioPlantioAsync(usuarioId, lavouraId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }
    }
}
