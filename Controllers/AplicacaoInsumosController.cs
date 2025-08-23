using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;

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
        public async Task<ActionResult<IEnumerable<AplicacaoInsumos>>> GetAplicacao_Insumos()
        {
            var usuarioId = GetUsuarioId();
            return await _context.Aplicacao_Insumos
                .Where(a => a.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // GET: api/AplicacaoInsumos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AplicacaoInsumos>> GetAplicacaoInsumos(int id)
        {
            var usuarioId = GetUsuarioId();
            var aplicacaoInsumos = await _context.Aplicacao_Insumos
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (aplicacaoInsumos == null)
            {
                return NotFound();
            }

            return aplicacaoInsumos;
        }

        // GET: api/Aplicacoes/porlavoura/5
        [HttpGet("porlavoura/{lavouraId}")]
        public async Task<ActionResult<IEnumerable<AplicacaoInsumos>>> GetAplicacoesPorLavoura(int lavouraId)
        {
            var usuarioId = GetUsuarioId();
            var aplicacoes = await _context.Aplicacao_Insumos
                .Where(a => a.lavouraID == lavouraId && a.UsuarioId == usuarioId)
                .ToListAsync();

            return aplicacoes;
        }

        // PUT: api/AplicacaoInsumos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAplicacaoInsumos(int id, AplicacaoInsumos aplicacaoInsumos)
        {
            var usuarioId = GetUsuarioId();
            
            if (id != aplicacaoInsumos.Id)
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

            // Verificar se houve mudança na quantidade
            if (aplicacaoInsumos.qtde != aplicacaoExistente.qtde)
            {
                // Se a nova quantidade for maior, verificar se há estoque suficiente
                if (aplicacaoInsumos.qtde > aplicacaoExistente.qtde)
                {
                    var quantidadeAdicional = aplicacaoInsumos.qtde - aplicacaoExistente.qtde;
                    var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelInsumoAsync(aplicacaoInsumos.insumoID, usuarioId);
                    
                    if (estoqueDisponivel < quantidadeAdicional)
                    {
                        return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Necessário: {quantidadeAdicional}" });
                    }
                    
                    // Baixar estoque adicional
                    await _estoqueService.BaixarEstoqueInsumoAsync(aplicacaoInsumos.insumoID, quantidadeAdicional, usuarioId);
                }
                // Se a nova quantidade for menor, retornar diferença ao estoque
                else if (aplicacaoInsumos.qtde < aplicacaoExistente.qtde)
                {
                    var quantidadeRetornar = aplicacaoExistente.qtde - aplicacaoInsumos.qtde;
                    await _estoqueService.RetornarEstoqueInsumoAsync(aplicacaoInsumos.insumoID, quantidadeRetornar, usuarioId);
                }
            }

            // Garantir que o UsuarioId não seja alterado
            aplicacaoInsumos.UsuarioId = usuarioId;

            _context.Entry(aplicacaoInsumos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AplicacaoInsumosExists(id, usuarioId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AplicacaoInsumos
        [HttpPost]
        public async Task<ActionResult<AplicacaoInsumos>> PostAplicacaoInsumos(AplicacaoInsumos aplicacaoInsumos)
        {
            var usuarioId = GetUsuarioId();
            aplicacaoInsumos.UsuarioId = usuarioId;
            
            // Verificar se há estoque suficiente
            if (!await _estoqueService.VerificarEstoqueInsumoAsync(aplicacaoInsumos.insumoID, aplicacaoInsumos.qtde, usuarioId))
            {
                var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelInsumoAsync(aplicacaoInsumos.insumoID, usuarioId);
                return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Solicitado: {aplicacaoInsumos.qtde}" });
            }
            
            // Baixar estoque
            await _estoqueService.BaixarEstoqueInsumoAsync(aplicacaoInsumos.insumoID, aplicacaoInsumos.qtde, usuarioId);
            
            _context.Aplicacao_Insumos.Add(aplicacaoInsumos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAplicacaoInsumos", new { id = aplicacaoInsumos.Id }, aplicacaoInsumos);
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

            // Retornar quantidade ao estoque
            await _estoqueService.RetornarEstoqueInsumoAsync(aplicacaoInsumos.insumoID, aplicacaoInsumos.qtde, usuarioId);

            _context.Aplicacao_Insumos.Remove(aplicacaoInsumos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AplicacaoInsumosExists(int id, int usuarioId)
        {
            return _context.Aplicacao_Insumos.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}
