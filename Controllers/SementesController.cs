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
    public class SementesController : ControllerBase
    {
        private readonly Contexto _context;

        public SementesController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Sementes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Semente>>> GetSementes()
        {
            return await _context.Sementes.ToListAsync();
        }

        // GET: api/Sementes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Semente>> GetSemente(int id)
        {
            var semente = await _context.Sementes.FindAsync(id);

            if (semente == null)
            {
                return NotFound();
            }

            return semente;
        }

        // PUT: api/Sementes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSemente(int id, Semente semente)
        {
            if (id != semente.Id)
            {
                return BadRequest();
            }

            _context.Entry(semente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SementeExists(id))
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

        // POST: api/Sementes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Semente>> PostSemente(Semente semente)
        {
            _context.Sementes.Add(semente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSemente", new { id = semente.Id }, semente);
        }

        // DELETE: api/Sementes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemente(int id)
        {
            var semente = await _context.Sementes.FindAsync(id);
            if (semente == null)
            {
                return NotFound();
            }

            _context.Sementes.Remove(semente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SementeExists(int id)
        {
            return _context.Sementes.Any(e => e.Id == id);
        }
    }
}
