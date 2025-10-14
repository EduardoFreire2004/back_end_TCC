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
    public class AplicacaoInsumosController : BaseController
    {
        private readonly Contexto _context;
        private readonly IEstoqueService _estoqueService;

        public AplicacaoInsumosController(Contexto context, IEstoqueService estoqueService)
        {
            _context = context;
            _estoqueService = estoqueService;
        }

        // GET: api/AplicacaoInsumos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AplicacaoInsumosResponseDto>>> GetAplicacao_Insumos()
        {
            var usuarioId = GetUsuarioId();
            var aplicacoes = await _context.Aplicacao_Insumos
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.lavoura)
                .Include(a => a.insumo)
                .Select(a => new AplicacaoInsumosResponseDto
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    lavouraID = a.lavouraID,
                    insumoID = a.insumoID,
                    qtde = a.qtde,
                    dataHora = a.dataHora,
                    descricao = a.descricao,
                    LavouraNome = a.lavoura.nome,
                    InsumoNome = a.insumo.nome,
                    InsumoUnidadeMedida = a.insumo.unidade_Medida
                })
                .ToListAsync();

            return aplicacoes;
        }

        // GET: api/AplicacaoInsumos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AplicacaoInsumosResponseDto>> GetAplicacaoInsumos(int id)
        {
            var usuarioId = GetUsuarioId();
            var aplicacaoInsumos = await _context.Aplicacao_Insumos
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .Include(a => a.lavoura)
                .Include(a => a.insumo)
                .Select(a => new AplicacaoInsumosResponseDto
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    lavouraID = a.lavouraID,
                    insumoID = a.insumoID,
                    qtde = a.qtde,
                    dataHora = a.dataHora,
                    descricao = a.descricao,
                    LavouraNome = a.lavoura.nome,
                    InsumoNome = a.insumo.nome,
                    InsumoUnidadeMedida = a.insumo.unidade_Medida
                })
                .FirstOrDefaultAsync();

            if (aplicacaoInsumos == null)
                return NotFound();

            return aplicacaoInsumos;
        }

        // GET: api/AplicacaoInsumos/porlavoura/5
        [HttpGet("porlavoura/{lavouraId}")]
        public async Task<ActionResult<IEnumerable<AplicacaoInsumosResponseDto>>> GetAplicacoesPorLavoura(int lavouraId)
        {
            var usuarioId = GetUsuarioId();
            var aplicacoes = await _context.Aplicacao_Insumos
                .Where(a => a.lavouraID == lavouraId && a.UsuarioId == usuarioId)
                .Include(a => a.lavoura)
                .Include(a => a.insumo)
                .Select(a => new AplicacaoInsumosResponseDto
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    lavouraID = a.lavouraID,
                    insumoID = a.insumoID,
                    qtde = a.qtde,
                    dataHora = a.dataHora,
                    descricao = a.descricao,
                    LavouraNome = a.lavoura.nome,
                    InsumoNome = a.insumo.nome,
                    InsumoUnidadeMedida = a.insumo.unidade_Medida
                })
                .ToListAsync();

            return aplicacoes;
        }

        // PUT: api/AplicacaoInsumos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAplicacaoInsumos(int id, AplicacaoInsumosUpdateDto dto)
        {
            var usuarioId = GetUsuarioId();

            if (id != dto.Id)
                return BadRequest();

            var aplicacaoExistente = await _context.Aplicacao_Insumos
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == usuarioId);

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
                        var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelInsumoAsync(dto.insumoID, usuarioId);

                        if (estoqueDisponivel < adicional)
                            return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Necessário: {adicional}" });

                        var baixou = await _estoqueService.BaixarEstoqueInsumoAsync(dto.insumoID, adicional, usuarioId);
                        if (!baixou)
                            return BadRequest(new { message = "Erro ao baixar estoque adicional" });
                    }
                    else
                    {
                        var devolver = aplicacaoExistente.qtde - dto.qtde;
                        var retornou = await _estoqueService.RetornarEstoqueInsumoAsync(dto.insumoID, devolver, usuarioId);
                        if (!retornou)
                            return BadRequest(new { message = "Erro ao retornar estoque" });
                    }
                }

                aplicacaoExistente.lavouraID = dto.lavouraID;
                aplicacaoExistente.insumoID = dto.insumoID;
                aplicacaoExistente.qtde = dto.qtde;
                aplicacaoExistente.dataHora = dto.dataHora;
                aplicacaoExistente.descricao = dto.descricao;

                _context.Entry(aplicacaoExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // 🔄 Atualizar movimentação associada
                var movimentacao = await _context.MovimentacoesEstoque
                    .FirstOrDefaultAsync(m => m.origemAplicacaoInsumoID == aplicacaoExistente.Id && m.UsuarioId == usuarioId);

                if (movimentacao != null)
                {
                    movimentacao.qtde = dto.qtde;
                    movimentacao.dataHora = dto.dataHora;
                    movimentacao.descricao = string.IsNullOrWhiteSpace(dto.descricao)
                        ? "Saída por Aplicação de Insumo"
                        : $"Saída por Aplicação de Insumo - {dto.descricao}";

                    _context.Entry(movimentacao).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao atualizar aplicação de insumo: {ex.Message}" });
            }

            return NoContent();
        }

        // POST: api/AplicacaoInsumos
        [HttpPost]
        public async Task<ActionResult<AplicacaoInsumosResponseDto>> PostAplicacaoInsumos(AplicacaoInsumosCreateDto dto)
        {
            var usuarioId = GetUsuarioId();

            var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelInsumoAsync(dto.insumoID, usuarioId);
            if (estoqueDisponivel < dto.qtde)
                return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Solicitado: {dto.qtde}" });

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var baixou = await _estoqueService.BaixarEstoqueInsumoAsync(dto.insumoID, dto.qtde, usuarioId);
                if (!baixou)
                    return BadRequest(new { message = "Erro ao baixar estoque" });

                var aplicacaoInsumos = new AplicacaoInsumos
                {
                    lavouraID = dto.lavouraID,
                    insumoID = dto.insumoID,
                    qtde = dto.qtde,
                    dataHora = dto.dataHora,
                    descricao = dto.descricao,
                    UsuarioId = usuarioId
                };

                _context.Aplicacao_Insumos.Add(aplicacaoInsumos);
                await _context.SaveChangesAsync();

                var movimentacao = new MovimentacaoEstoque
                {
                    lavouraID = aplicacaoInsumos.lavouraID,
                    movimentacao = TipoMovimentacao.Saida,
                    insumoID = aplicacaoInsumos.insumoID,
                    qtde = aplicacaoInsumos.qtde,
                    dataHora = aplicacaoInsumos.dataHora,
                    descricao = string.IsNullOrWhiteSpace(aplicacaoInsumos.descricao)
                        ? "Saída por Aplicação de Insumo"
                        : $"Saída por Aplicação de Insumo - {aplicacaoInsumos.descricao}",
                    UsuarioId = usuarioId,
                    origemAplicacaoInsumoID = aplicacaoInsumos.Id
                };

                _context.MovimentacoesEstoque.Add(movimentacao);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var criado = await _context.Aplicacao_Insumos
                    .Include(a => a.lavoura)
                    .Include(a => a.insumo)
                    .Where(a => a.Id == aplicacaoInsumos.Id)
                    .Select(a => new AplicacaoInsumosResponseDto
                    {
                        Id = a.Id,
                        UsuarioId = a.UsuarioId,
                        lavouraID = a.lavouraID,
                        insumoID = a.insumoID,
                        qtde = a.qtde,
                        dataHora = a.dataHora,
                        descricao = a.descricao,
                        LavouraNome = a.lavoura.nome,
                        InsumoNome = a.insumo.nome,
                        InsumoUnidadeMedida = a.insumo.unidade_Medida
                    })
                    .FirstOrDefaultAsync();

                return CreatedAtAction("GetAplicacaoInsumos", new { id = aplicacaoInsumos.Id }, criado);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao criar aplicação de insumo: {ex.Message}" });
            }
        }

        // DELETE: api/AplicacaoInsumos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAplicacaoInsumos(int id)
        {
            var usuarioId = GetUsuarioId();

            var aplicacaoInsumos = await _context.Aplicacao_Insumos
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == usuarioId);

            if (aplicacaoInsumos == null)
                return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var retornou = await _estoqueService.RetornarEstoqueInsumoAsync(aplicacaoInsumos.insumoID, aplicacaoInsumos.qtde, usuarioId);
                if (!retornou)
                    return BadRequest(new { message = "Erro ao retornar estoque" });

                // 🧹 Excluir movimentação vinculada
                var movimentacao = await _context.MovimentacoesEstoque
                    .FirstOrDefaultAsync(m => m.origemAplicacaoInsumoID == aplicacaoInsumos.Id && m.UsuarioId == usuarioId);

                if (movimentacao != null)
                {
                    _context.MovimentacoesEstoque.Remove(movimentacao);
                    await _context.SaveChangesAsync();
                }

                _context.Aplicacao_Insumos.Remove(aplicacaoInsumos);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao excluir aplicação de insumo: {ex.Message}" });
            }

            return NoContent();
        }
    }
}
