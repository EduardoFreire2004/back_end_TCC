using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using Microsoft.AspNetCore.Authorization;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgrotoxicosController : ControllerBase
    {
        private readonly Contexto _context;

        public AgrotoxicosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Agrotoxicos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agrotoxico>>> GetAgrotoxicos()
        {
            return await _context.Agrotoxicos.ToListAsync();
        }

        // GET: api/Agrotoxicos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agrotoxico>> GetAgrotoxico(int id)
        {
            var agrotoxico = await _context.Agrotoxicos.FindAsync(id);

            if (agrotoxico == null)
            {
                return NotFound();
            }

            return agrotoxico;
        }

        // PUT: api/Agrotoxicos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgrotoxico(int id, Agrotoxico agrotoxico)
        {
            if (id != agrotoxico.Id)
            {
                return BadRequest();
            }

            _context.Entry(agrotoxico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgrotoxicoExists(id))
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

        // POST: api/Agrotoxicos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Agrotoxico>> PostAgrotoxico(Agrotoxico agrotoxico)
        {
            _context.Agrotoxicos.Add(agrotoxico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgrotoxico", new { id = agrotoxico.Id }, agrotoxico);
        }

        // DELETE: api/Agrotoxicos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgrotoxico(int id)
        {
            var agrotoxico = await _context.Agrotoxicos.FindAsync(id);
            if (agrotoxico == null)
            {
                return NotFound();
            }

            _context.Agrotoxicos.Remove(agrotoxico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgrotoxicoExists(int id)
        {
            return _context.Agrotoxicos.Any(e => e.Id == id);
        }
    }
}
