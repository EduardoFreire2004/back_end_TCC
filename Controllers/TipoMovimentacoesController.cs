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
    [ApiController]
    public class TipoMovimentacoesController : ControllerBase
    {
        private readonly Contexto _context;

        public TipoMovimentacoesController(Contexto context)
        {
            _context = context;
        }

        // GET: api/TipoMovimentacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoMovimentacao>>> GetTipo_Movimentacoes()
        {
            return await _context.Tipo_Movimentacoes.ToListAsync();
        }

        // GET: api/TipoMovimentacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoMovimentacao>> GetTipoMovimentacao(int id)
        {
            var tipoMovimentacao = await _context.Tipo_Movimentacoes.FindAsync(id);

            if (tipoMovimentacao == null)
            {
                return NotFound();
            }

            return tipoMovimentacao;
        }

        // PUT: api/TipoMovimentacoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoMovimentacao(int id, TipoMovimentacao tipoMovimentacao)
        {
            if (id != tipoMovimentacao.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoMovimentacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoMovimentacaoExists(id))
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

        // POST: api/TipoMovimentacoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoMovimentacao>> PostTipoMovimentacao(TipoMovimentacao tipoMovimentacao)
        {
            _context.Tipo_Movimentacoes.Add(tipoMovimentacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoMovimentacao", new { id = tipoMovimentacao.Id }, tipoMovimentacao);
        }

        // DELETE: api/TipoMovimentacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoMovimentacao(int id)
        {
            var tipoMovimentacao = await _context.Tipo_Movimentacoes.FindAsync(id);
            if (tipoMovimentacao == null)
            {
                return NotFound();
            }

            _context.Tipo_Movimentacoes.Remove(tipoMovimentacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoMovimentacaoExists(int id)
        {
            return _context.Tipo_Movimentacoes.Any(e => e.Id == id);
        }
    }
}
