using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using API_TCC.DTOs;
using API_TCC.Enums;

namespace API_TCC.Controllers
{
    [Route("api/relatorios")]
    [ApiController]
    public class RelatoriosController : BaseController
    {
        private readonly Contexto _context;

        public RelatoriosController(Contexto context)
        {
            _context = context;
        }

        #region === RELATÓRIOS POR LAVOURA ===

        [HttpGet("aplicacao/{lavouraId}")]
        public async Task<IActionResult> RelatorioAplicacao(
            int lavouraId,
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            if (dataInicio == default || dataFim == default)
                return BadRequest("Data de início e fim são obrigatórias.");
            if (dataInicio > dataFim)
                return BadRequest("A data inicial não pode ser maior que a final.");

            var usuarioId = GetUsuarioId();

            var dados = await _context.Aplicacoes
                .Include(x => x.agrotoxico)
                .Include(x => x.lavoura)
                .Include(x => x.agrotoxico)
                .Where(x => x.UsuarioId == usuarioId
                         && x.lavouraID == lavouraId
                         && x.dataHora >= dataInicio && x.dataHora <= dataFim)
                .Select(x => new RelatorioAplicacaoDTO
                {
                    Id = x.Id,
                    Lavoura = x.lavoura.nome,
                    Agrotoxico = x.agrotoxico.nome,
                    Fornecedor = x.agrotoxico.fornecedor.nome,
                    Quantidade = x.qtde,
                    DataHora = x.dataHora,
                })
                .ToListAsync();

            return Ok(dados);
        }

        [HttpGet("aplicacao-insumo/{lavouraId}")]
        public async Task<IActionResult> RelatorioAplicacaoInsumo(
            int lavouraId,
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            if (dataInicio == default || dataFim == default)
                return BadRequest("Data de início e fim são obrigatórias.");
            if (dataInicio > dataFim)
                return BadRequest("A data inicial não pode ser maior que a final.");

            var usuarioId = GetUsuarioId();

            var dados = await _context.Aplicacao_Insumos
                .Include(x => x.insumo)
                .Include(x => x.lavoura)
                .Include(x => x.insumo)
                .Where(x => x.UsuarioId == usuarioId
                         && x.lavouraID == lavouraId
                         && x.dataHora >= dataInicio && x.dataHora <= dataFim)
                .Select(x => new RelatorioAplicacaoInsumoDTO
                {
                    Id = x.Id,
                    Lavoura = x.lavoura.nome,
                    Insumo = x.insumo.nome,
                    Fornecedor = x.insumo.fornecedor.nome,
                    Quantidade = x.qtde,
                    DataHora = x.dataHora
                })
                .ToListAsync();

            return Ok(dados);
        }

        [HttpGet("plantio/{lavouraId}")]
        public async Task<IActionResult> RelatorioPlantio(
            int lavouraId,
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            if (dataInicio == default || dataFim == default)
                return BadRequest("Data de início e fim são obrigatórias.");
            if (dataInicio > dataFim)
                return BadRequest("A data inicial não pode ser maior que a final.");

            var usuarioId = GetUsuarioId();

            var dados = await _context.Plantios
                .Include(x => x.semente)
                .Include(x => x.lavoura)
                .Where(x => x.UsuarioId == usuarioId
                         && x.lavouraID == lavouraId
                         && x.dataHora >= dataInicio && x.dataHora <= dataFim)
                .Select(x => new RelatorioPlantioDTO
                {
                    Id = x.Id,
                    Lavoura = x.lavoura.nome,
                    Semente = x.semente.nome,
                    Fornecedor = x.semente.fornecedor.nome,
                    AreaPlantada = x.areaPlantada,
                    Quantidade = x.qtde,
                    DataHora = x.dataHora
                })
                .ToListAsync();

            return Ok(dados);
        }

        [HttpGet("colheita/{lavouraId}")]
        public async Task<IActionResult> RelatorioColheita(
            int lavouraId,
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            if (dataInicio == default || dataFim == default)
                return BadRequest("Data de início e fim são obrigatórias.");
            if (dataInicio > dataFim)
                return BadRequest("A data inicial não pode ser maior que a final.");

            var usuarioId = GetUsuarioId();

            var dados = await _context.Colheitas
                .Include(x => x.lavoura)
                .Where(x => x.UsuarioId == usuarioId
                         && x.lavouraID == lavouraId
                         && x.dataHora >= dataInicio && x.dataHora <= dataFim)
                .Select(x => new RelatorioColheitaDTO
                {
                    Id = x.Id,
                    Lavoura = x.lavoura.nome,
                    QuantidadeSacas = x.quantidadeSacas,
                    AreaHa = x.areaHectares,
                    CooperativaDestino = x.cooperativaDestino,
                    PrecoSaca = x.precoPorSaca,
                    DataHora = x.dataHora
                })
                .ToListAsync();

            return Ok(dados);
        }

    [HttpGet("movimentacao-estoque/{lavouraId}")]
    public async Task<IActionResult> RelatorioMovimentacaoEstoque(
    int lavouraId,
    [FromQuery] DateTime dataInicio,
    [FromQuery] DateTime dataFim)
        {
            if (dataInicio == default || dataFim == default)
                return BadRequest("Data de início e fim são obrigatórias.");
            if (dataInicio > dataFim)
                return BadRequest("A data inicial não pode ser maior que a final.");

            var usuarioId = GetUsuarioId();

            var dados = await _context.MovimentacoesEstoque
                .Include(x => x.Insumo)
                .Include(x => x.Agrotoxico)
                .Include(x => x.Lavoura)
                .Where(x => x.UsuarioId == usuarioId
                         && x.lavouraID == lavouraId
                         && x.dataHora >= dataInicio && x.dataHora <= dataFim)
                .Select(x => new RelatorioMovimentacaoDTO
                {
                    Id = x.Id,
                    Lavoura = x.Lavoura.nome,
                    Item = x.Insumo != null ? x.Insumo.nome :
                           (x.Agrotoxico != null ? x.Agrotoxico.nome : "—"),
                    Tipo = x.movimentacao.ToString(),
                    Quantidade = x.qtde,
                    DataHora = x.dataHora
                })
                .ToListAsync();

            return Ok(dados);
        }

        #endregion

        #region === RELATÓRIOS DE ITENS CADASTRADOS ===

        [HttpGet("agrotoxico")]
        public async Task<IActionResult> RelatorioAgrotoxico()
        {
            var usuarioId = GetUsuarioId();

            var dados = await _context.Agrotoxicos
                .Include(x => x.fornecedor)
                .Include(x => x.tipo)
                .Where(x => x.UsuarioId == usuarioId)
                .Select(x => new RelatorioAgrotoxicoDTO
                {
                    Id = x.Id,
                    nome = x.nome,
                    tipo = x.tipo.descricao,
                    fonecedor = x.fornecedor.nome,
                    unidadeMedida = x.unidade_Medida,
                    qtde = x.qtde,
                    preco = x.preco,
                    DataHora = x.data_Cadastro
                })
                .ToListAsync();

            return Ok(dados);
        }

        [HttpGet("insumo")]
        public async Task<IActionResult> RelatorioInsumo()
        {
            var usuarioId = GetUsuarioId();

            var dados = await _context.Insumos
                .Include(x => x.fornecedor)
                .Include(x => x.categoriaInsumo)
                .Where(x => x.UsuarioId == usuarioId)
                .Select(x => new RelatorioInsumoDTO
                {
                    Id = x.Id,
                    nome = x.nome,
                    categoria = x.categoriaInsumo.descricao,
                    fonecedor = x.fornecedor.nome,
                    unidadeMedida = x.unidade_Medida,
                    qtde = x.qtde,
                    preco = x.preco,
                    DataHora = x.data_Cadastro
                })
                .ToListAsync();

            return Ok(dados);
        }

        [HttpGet("semente")]
        public async Task<IActionResult> RelatorioSemente()
        {
            var usuarioId = GetUsuarioId();

            var dados = await _context.Sementes
                .Include(x => x.fornecedor)
                .Where(x => x.UsuarioId == usuarioId)
                .Select(x => new RelatorioSementeDTO
                {
                    Id = x.Id,
                    nome = x.nome,
                    tipo = x.tipo,
                    fonecedor = x.fornecedor.nome,
                    marca = x.marca,
                    preco = x.preco,
                    DataHora = x.data_Cadastro
                })
                .ToListAsync();

            return Ok(dados);
        }

        #endregion
    }
}
