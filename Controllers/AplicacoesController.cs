using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using API_TCC.DTOs;
using API_TCC.Enums;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    public class AplicacoesController : BaseController
    {
        private readonly Contexto _context;
        private readonly IEstoqueService _estoqueService;

        public AplicacoesController(Contexto context, IEstoqueService estoqueService)
        {
            _context = context;
            _estoqueService = estoqueService;
        }

        // GET: api/Aplicacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AplicacaoResponseDto>>> GetAplicacoes()
        {
            var usuarioId = GetUsuarioId();
            var aplicacoes = await _context.Aplicacoes
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.lavoura)
                .Include(a => a.agrotoxico)
                .Select(a => new AplicacaoResponseDto
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    lavouraID = a.lavouraID,
                    agrotoxicoID = a.agrotoxicoID,
                    qtde = a.qtde,
                    dataHora = a.dataHora,
                    descricao = a.descricao,
                    LavouraNome = a.lavoura.nome,
                    AgrotoxicoNome = a.agrotoxico.nome,
                    AgrotoxicoUnidadeMedida = a.agrotoxico.unidade_Medida
                })
                .ToListAsync();

            return aplicacoes;
        }

        // GET: api/Aplicacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AplicacaoResponseDto>> GetAplicacao(int id)
        {
            var usuarioId = GetUsuarioId();
            var aplicacao = await _context.Aplicacoes
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .Include(a => a.lavoura)
                .Include(a => a.agrotoxico)
                .Select(a => new AplicacaoResponseDto
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    lavouraID = a.lavouraID,
                    agrotoxicoID = a.agrotoxicoID,
                    qtde = a.qtde,
                    dataHora = a.dataHora,
                    descricao = a.descricao,
                    LavouraNome = a.lavoura.nome,
                    AgrotoxicoNome = a.agrotoxico.nome,
                    AgrotoxicoUnidadeMedida = a.agrotoxico.unidade_Medida
                })
                .FirstOrDefaultAsync();

            if (aplicacao == null)
            {
                return NotFound();
            }

            return aplicacao;
        }

        // GET: api/Aplicacoes/porlavoura/5
        [HttpGet("porlavoura/{lavouraId}")]
        public async Task<ActionResult<IEnumerable<AplicacaoResponseDto>>> GetAplicacoesPorLavoura(int lavouraId)
        {
            var usuarioId = GetUsuarioId();
            var aplicacoes = await _context.Aplicacoes
                .Where(a => a.lavouraID == lavouraId && a.UsuarioId == usuarioId)
                .Include(a => a.lavoura)
                .Include(a => a.agrotoxico)
                .Select(a => new AplicacaoResponseDto
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    lavouraID = a.lavouraID,
                    agrotoxicoID = a.agrotoxicoID,
                    qtde = a.qtde,
                    dataHora = a.dataHora,
                    descricao = a.descricao,
                    LavouraNome = a.lavoura.nome,
                    AgrotoxicoNome = a.agrotoxico.nome,
                    AgrotoxicoUnidadeMedida = a.agrotoxico.unidade_Medida
                })
                .ToListAsync();

            return aplicacoes;
        }

        // PUT: api/Aplicacoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAplicacao(int id, AplicacaoUpdateDto dto)
        {
            var usuarioId = GetUsuarioId();

            if (id != dto.Id)
                return BadRequest();

            var aplicacaoExistente = await _context.Aplicacoes
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (aplicacaoExistente == null)
                return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (dto.qtde != aplicacaoExistente.qtde)
                {
                    if (dto.qtde > aplicacaoExistente.qtde)
                    {
                        var adicional = dto.qtde - aplicacaoExistente.qtde;
                        var estoque = await _estoqueService.ObterEstoqueDisponivelAgrotoxicoAsync(dto.agrotoxicoID, usuarioId);

                        if (estoque < adicional)
                            return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoque}, Necessário: {adicional}" });

                        await _estoqueService.BaixarEstoqueAgrotoxicoAsync(dto.agrotoxicoID, adicional, usuarioId);
                    }
                    else
                    {
                        var retorno = aplicacaoExistente.qtde - dto.qtde;
                        await _estoqueService.RetornarEstoqueAgrotoxicoAsync(dto.agrotoxicoID, retorno, usuarioId);
                    }
                }

                aplicacaoExistente.lavouraID = dto.lavouraID;
                aplicacaoExistente.agrotoxicoID = dto.agrotoxicoID;
                aplicacaoExistente.qtde = dto.qtde;
                aplicacaoExistente.dataHora = dto.dataHora;
                aplicacaoExistente.descricao = dto.descricao;

                _context.Entry(aplicacaoExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Atualizar movimentação associada
                var mov = await _context.MovimentacoesEstoque
                    .FirstOrDefaultAsync(m => m.origemAplicacaoID == aplicacaoExistente.Id && m.UsuarioId == usuarioId);

                if (mov != null)
                {
                    mov.qtde = dto.qtde;
                    mov.dataHora = dto.dataHora;
                    mov.descricao = string.IsNullOrWhiteSpace(dto.descricao)
                        ? "Saída por Aplicação de Agrotóxico"
                        : $"Saída por Aplicação de Agrotóxico - {dto.descricao}";

                    _context.Entry(mov).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao atualizar aplicação: {ex.Message}" });
            }

            return NoContent();
        }

        // POST: api/Aplicacoes
        [HttpPost]
        public async Task<ActionResult<AplicacaoResponseDto>> PostAplicacao(AplicacaoCreateDto dto)
        {
            var usuarioId = GetUsuarioId();

            var estoque = await _estoqueService.ObterEstoqueDisponivelAgrotoxicoAsync(dto.agrotoxicoID, usuarioId);
            if (estoque < dto.qtde)
                return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoque}, Solicitado: {dto.qtde}" });

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _estoqueService.BaixarEstoqueAgrotoxicoAsync(dto.agrotoxicoID, dto.qtde, usuarioId);

                var aplicacao = new Aplicacao
                {
                    lavouraID = dto.lavouraID,
                    agrotoxicoID = dto.agrotoxicoID,
                    qtde = dto.qtde,
                    dataHora = dto.dataHora,
                    descricao = dto.descricao,
                    UsuarioId = usuarioId
                };

                _context.Aplicacoes.Add(aplicacao);
                await _context.SaveChangesAsync();

                var mov = new MovimentacaoEstoque
                {
                    lavouraID = aplicacao.lavouraID,
                    movimentacao = TipoMovimentacao.Saida,
                    agrotoxicoID = aplicacao.agrotoxicoID,
                    qtde = aplicacao.qtde,
                    dataHora = aplicacao.dataHora,
                    descricao = string.IsNullOrWhiteSpace(aplicacao.descricao)
                        ? "Saída por Aplicação de Agrotóxico"
                        : $"Saída por Aplicação de Agrotóxico - {aplicacao.descricao}",
                    UsuarioId = usuarioId,
                    origemAplicacaoID = aplicacao.Id
                };

                _context.MovimentacoesEstoque.Add(mov);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var response = await _context.Aplicacoes
                    .Where(a => a.Id == aplicacao.Id)
                    .Include(a => a.lavoura)
                    .Include(a => a.agrotoxico)
                    .Select(a => new AplicacaoResponseDto
                    {
                        Id = a.Id,
                        UsuarioId = a.UsuarioId,
                        lavouraID = a.lavouraID,
                        agrotoxicoID = a.agrotoxicoID,
                        qtde = a.qtde,
                        dataHora = a.dataHora,
                        descricao = a.descricao,
                        LavouraNome = a.lavoura.nome,
                        AgrotoxicoNome = a.agrotoxico.nome,
                        AgrotoxicoUnidadeMedida = a.agrotoxico.unidade_Medida
                    })
                    .FirstOrDefaultAsync();

                return CreatedAtAction("GetAplicacao", new { id = aplicacao.Id }, response);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao criar aplicação: {ex.Message}" });
            }
        }

        // DELETE: api/Aplicacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAplicacao(int id)
        {
            var usuarioId = GetUsuarioId();
            var aplicacao = await _context.Aplicacoes
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == usuarioId);

            if (aplicacao == null)
                return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _estoqueService.RetornarEstoqueAgrotoxicoAsync(aplicacao.agrotoxicoID, aplicacao.qtde, usuarioId);

                _context.Aplicacoes.Remove(aplicacao);
                await _context.SaveChangesAsync();

                var mov = await _context.MovimentacoesEstoque
                    .FirstOrDefaultAsync(m => m.origemAplicacaoID == aplicacao.Id && m.UsuarioId == usuarioId);

                if (mov != null)
                {
                    _context.MovimentacoesEstoque.Remove(mov);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao excluir aplicação: {ex.Message}" });
            }

            return NoContent();
        }
    }
}
