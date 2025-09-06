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
        /// Gera relatório de fornecedores em formato JSON
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <returns>Lista de fornecedores com estatísticas</returns>
        [HttpGet("fornecedores/{usuarioId}")]
        public async Task<IActionResult> RelatorioFornecedores(int usuarioId)
        {
            var resultado = await _relatorioService.GerarRelatorioFornecedoresAsync(usuarioId);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Gera relatório de aplicações de agrotóxicos em formato JSON
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="dataInicio">Data de início (opcional)</param>
        /// <param name="dataFim">Data de fim (opcional)</param>
        /// <returns>Lista de aplicações de agrotóxicos</returns>
        [HttpGet("aplicacao/{usuarioId}")]
        public async Task<IActionResult> RelatorioAplicacao(int usuarioId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var resultado = await _relatorioService.GerarRelatorioAplicacaoAsync(usuarioId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Gera relatório de aplicações de insumos em formato JSON
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="dataInicio">Data de início (opcional)</param>
        /// <param name="dataFim">Data de fim (opcional)</param>
        /// <returns>Lista de aplicações de insumos</returns>
        [HttpGet("aplicacao-insumo/{usuarioId}")]
        public async Task<IActionResult> RelatorioAplicacaoInsumo(int usuarioId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var resultado = await _relatorioService.GerarRelatorioAplicacaoInsumoAsync(usuarioId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Gera relatório de sementes em formato JSON
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="dataInicio">Data de início (opcional)</param>
        /// <param name="dataFim">Data de fim (opcional)</param>
        /// <returns>Lista de sementes</returns>
        [HttpGet("semente/{usuarioId}")]
        public async Task<IActionResult> RelatorioSemente(int usuarioId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var resultado = await _relatorioService.GerarRelatorioSementeAsync(usuarioId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Gera relatório de insumos em formato JSON
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="dataInicio">Data de início (opcional)</param>
        /// <param name="dataFim">Data de fim (opcional)</param>
        /// <returns>Lista de insumos</returns>
        [HttpGet("insumo/{usuarioId}")]
        public async Task<IActionResult> RelatorioInsumo(int usuarioId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var resultado = await _relatorioService.GerarRelatorioInsumoAsync(usuarioId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Gera relatório de agrotóxicos em formato JSON
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="dataInicio">Data de início (opcional)</param>
        /// <param name="dataFim">Data de fim (opcional)</param>
        /// <returns>Lista de agrotóxicos</returns>
        [HttpGet("agrotoxico/{usuarioId}")]
        public async Task<IActionResult> RelatorioAgrotoxico(int usuarioId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var resultado = await _relatorioService.GerarRelatorioAgrotoxicoAsync(usuarioId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Gera relatório de colheitas em formato JSON
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="dataInicio">Data de início (opcional)</param>
        /// <param name="dataFim">Data de fim (opcional)</param>
        /// <returns>Lista de colheitas</returns>
        [HttpGet("colheita/{usuarioId}")]
        public async Task<IActionResult> RelatorioColheita(int usuarioId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var resultado = await _relatorioService.GerarRelatorioColheitaAsync(usuarioId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Gera relatório de movimentação de estoque em formato JSON
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="dataInicio">Data de início (opcional)</param>
        /// <param name="dataFim">Data de fim (opcional)</param>
        /// <returns>Lista de movimentações de estoque</returns>
        [HttpGet("movimentacao-estoque/{usuarioId}")]
        public async Task<IActionResult> RelatorioMovimentacaoEstoque(int usuarioId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var resultado = await _relatorioService.GerarRelatorioMovimentacaoEstoqueAsync(usuarioId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Gera relatório de plantios em formato JSON
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="dataInicio">Data de início (opcional)</param>
        /// <param name="dataFim">Data de fim (opcional)</param>
        /// <returns>Lista de plantios</returns>
        [HttpGet("plantio/{usuarioId}")]
        public async Task<IActionResult> RelatorioPlantio(int usuarioId, [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
        {
            var resultado = await _relatorioService.GerarRelatorioPlantioAsync(usuarioId, dataInicio, dataFim);
            return resultado.Success ? Ok(resultado) : BadRequest(resultado);
        }
    }
}