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
    public class AplicacoesController : ControllerBase
    {
        private readonly Contexto _context;

        public AplicacoesController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Aplicacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aplicacao>>> GetAplicacoes()
        {
            return await _context.Aplicacoes.ToListAsync();
        }

        // GET: api/Aplicacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aplicacao>> GetAplicacao(int id)
        {
            var aplicacao = await _context.Aplicacoes.FindAsync(id);

            if (aplicacao == null)
            {
                return NotFound();
            }

            return aplicacao;
        }

        // PUT: api/Aplicacoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAplicacao(int id, Aplicacao aplicacao)
        {
            if (id != aplicacao.Id)
            {
                return BadRequest();
            }

            _context.Entry(aplicacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AplicacaoExists(id))
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

        // POST: api/Aplicacoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Aplicacao>> PostAplicacao(Aplicacao aplicacao)
        {
            _context.Aplicacoes.Add(aplicacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAplicacao", new { id = aplicacao.Id }, aplicacao);
        }

        // DELETE: api/Aplicacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAplicacao(int id)
        {
            var aplicacao = await _context.Aplicacoes.FindAsync(id);
            if (aplicacao == null)
            {
                return NotFound();
            }

            _context.Aplicacoes.Remove(aplicacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AplicacaoExists(int id)
        {
            return _context.Aplicacoes.Any(e => e.Id == id);
        }
    }
}
