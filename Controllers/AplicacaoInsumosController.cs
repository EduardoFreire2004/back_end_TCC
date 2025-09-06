using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using API_TCC.DTOs;

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
            {
                return NotFound();
            }

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
            {
                return BadRequest();
            }

            // Verificar se a aplicação pertence ao usuário
            var aplicacaoExistente = await _context.Aplicacao_Insumos
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (aplicacaoExistente == null)
            {
                return NotFound();
            }

            // Usar transação para garantir consistência
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Verificar se houve mudança na quantidade
                if (dto.qtde != aplicacaoExistente.qtde)
                {
                    // Se a nova quantidade for maior, verificar se há estoque suficiente
                    if (dto.qtde > aplicacaoExistente.qtde)
                    {
                        var quantidadeAdicional = dto.qtde - aplicacaoExistente.qtde;
                        var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelInsumoAsync(dto.insumoID, usuarioId);
                        
                        if (estoqueDisponivel < quantidadeAdicional)
                        {
                            return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Necessário: {quantidadeAdicional}" });
                        }
                        
                        // Baixar estoque adicional
                        var baixouEstoque = await _estoqueService.BaixarEstoqueInsumoAsync(dto.insumoID, quantidadeAdicional, usuarioId);
                        if (!baixouEstoque)
                        {
                            return BadRequest(new { message = "Erro ao baixar estoque adicional" });
                        }
                    }
                    // Se a nova quantidade for menor, retornar diferença ao estoque
                    else if (dto.qtde < aplicacaoExistente.qtde)
                    {
                        var quantidadeRetornar = aplicacaoExistente.qtde - dto.qtde;
                        var retornouEstoque = await _estoqueService.RetornarEstoqueInsumoAsync(dto.insumoID, quantidadeRetornar, usuarioId);
                        if (!retornouEstoque)
                        {
                            return BadRequest(new { message = "Erro ao retornar estoque" });
                        }
                    }
                }

                // Atualizar a aplicação
                aplicacaoExistente.lavouraID = dto.lavouraID;
                aplicacaoExistente.insumoID = dto.insumoID;
                aplicacaoExistente.qtde = dto.qtde;
                aplicacaoExistente.dataHora = dto.dataHora;
                aplicacaoExistente.descricao = dto.descricao;

                _context.Entry(aplicacaoExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                // Commit da transação
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao atualizar aplicação: {ex.Message}" });
            }

            return NoContent();
        }

        // POST: api/AplicacaoInsumos
        [HttpPost]
        public async Task<ActionResult<AplicacaoInsumosResponseDto>> PostAplicacaoInsumos(AplicacaoInsumosCreateDto dto)
        {
            var usuarioId = GetUsuarioId();
            
            // Verificar se há estoque suficiente
            var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelInsumoAsync(dto.insumoID, usuarioId);
            if (estoqueDisponivel < dto.qtde)
            {
                return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Solicitado: {dto.qtde}" });
            }
            
            // Usar transação para garantir consistência
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Baixar estoque
                var baixouEstoque = await _estoqueService.BaixarEstoqueInsumoAsync(dto.insumoID, dto.qtde, usuarioId);
                if (!baixouEstoque)
                {
                    return BadRequest(new { message = "Erro ao baixar estoque" });
                }
                
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
                
                // Commit da transação
                await transaction.CommitAsync();
                
                // Retornar a aplicação criada com dados de navegação
                var aplicacaoCriada = await _context.Aplicacao_Insumos
                    .Where(a => a.Id == aplicacaoInsumos.Id)
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

                return CreatedAtAction("GetAplicacaoInsumos", new { id = aplicacaoInsumos.Id }, aplicacaoCriada);
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao criar aplicação: {ex.Message}" });
            }
        }

        // DELETE: api/AplicacaoInsumos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAplicacaoInsumos(int id)
        {
            var usuarioId = GetUsuarioId();
            var aplicacaoInsumos = await _context.Aplicacao_Insumos
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (aplicacaoInsumos == null)
            {
                return NotFound();
            }

            // Usar transação para garantir consistência
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Retornar quantidade ao estoque
                var retornouEstoque = await _estoqueService.RetornarEstoqueInsumoAsync(aplicacaoInsumos.insumoID, aplicacaoInsumos.qtde, usuarioId);
                if (!retornouEstoque)
                {
                    return BadRequest(new { message = "Erro ao retornar estoque" });
                }

                _context.Aplicacao_Insumos.Remove(aplicacaoInsumos);
                await _context.SaveChangesAsync();
                
                // Commit da transação
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao excluir aplicação: {ex.Message}" });
            }

            return NoContent();
        }

        private bool AplicacaoInsumosExists(int id, int usuarioId)
        {
            return _context.Aplicacao_Insumos.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}
