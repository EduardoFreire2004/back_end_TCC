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
    public class LavourasController : ControllerBase
    {
        private readonly Contexto _context;

        public LavourasController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Lavouras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lavoura>>> GetLavouras()
        {
            return await _context.Lavouras.ToListAsync();
        }

        // GET: api/Lavouras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lavoura>> GetLavoura(int id)
        {
            var lavoura = await _context.Lavouras.FindAsync(id);

            if (lavoura == null)
            {
                return NotFound();
            }

            return lavoura;
        }

        // PUT: api/Lavouras/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLavoura(int id, Lavoura lavoura)
        {
            if (id != lavoura.Id)
            {
                return BadRequest();
            }

            _context.Entry(lavoura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LavouraExists(id))
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

        // POST: api/Lavouras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lavoura>> PostLavoura(Lavoura lavoura)
        {
            _context.Lavouras.Add(lavoura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLavoura", new { id = lavoura.Id }, lavoura);
        }

        // DELETE: api/Lavouras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLavoura(int id)
        {
            var lavoura = await _context.Lavouras.FindAsync(id);
            if (lavoura == null)
            {
                return NotFound();
            }

            _context.Lavouras.Remove(lavoura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LavouraExists(int id)
        {
            return _context.Lavouras.Any(e => e.Id == id);
        }
    }
}
