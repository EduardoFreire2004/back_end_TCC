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
    public class ColheitasController : ControllerBase
    {
        private readonly Contexto _context;

        public ColheitasController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Colheitas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colheita>>> GetColheitas()
        {
            return await _context.Colheitas.ToListAsync();
        }

        // GET: api/Colheitas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Colheita>> GetColheita(int id)
        {
            var colheita = await _context.Colheitas.FindAsync(id);

            if (colheita == null)
            {
                return NotFound();
            }

            return colheita;
        }

        // PUT: api/Colheitas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColheita(int id, Colheita colheita)
        {
            if (id != colheita.Id)
            {
                return BadRequest();
            }

            _context.Entry(colheita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColheitaExists(id))
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

        // POST: api/Colheitas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Colheita>> PostColheita(Colheita colheita)
        {
            _context.Colheitas.Add(colheita);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetColheita", new { id = colheita.Id }, colheita);
        }

        // DELETE: api/Colheitas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColheita(int id)
        {
            var colheita = await _context.Colheitas.FindAsync(id);
            if (colheita == null)
            {
                return NotFound();
            }

            _context.Colheitas.Remove(colheita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColheitaExists(int id)
        {
            return _context.Colheitas.Any(e => e.Id == id);
        }
    }
}
