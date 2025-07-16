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
    public class TipoAgrotoxicosController : ControllerBase
    {
        private readonly Contexto _context;

        public TipoAgrotoxicosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/TipoAgrotoxicos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoAgrotoxico>>> GetTipo_Agrotoxicos()
        {
            return await _context.Tipo_Agrotoxicos.ToListAsync();
        }

        // GET: api/TipoAgrotoxicos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoAgrotoxico>> GetTipoAgrotoxico(int id)
        {
            var tipoAgrotoxico = await _context.Tipo_Agrotoxicos.FindAsync(id);

            if (tipoAgrotoxico == null)
            {
                return NotFound();
            }

            return tipoAgrotoxico;
        }

        // PUT: api/TipoAgrotoxicos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoAgrotoxico(int id, TipoAgrotoxico tipoAgrotoxico)
        {
            if (id != tipoAgrotoxico.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoAgrotoxico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoAgrotoxicoExists(id))
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

        // POST: api/TipoAgrotoxicos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoAgrotoxico>> PostTipoAgrotoxico(TipoAgrotoxico tipoAgrotoxico)
        {
            _context.Tipo_Agrotoxicos.Add(tipoAgrotoxico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoAgrotoxico", new { id = tipoAgrotoxico.Id }, tipoAgrotoxico);
        }

        // DELETE: api/TipoAgrotoxicos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoAgrotoxico(int id)
        {
            var tipoAgrotoxico = await _context.Tipo_Agrotoxicos.FindAsync(id);
            if (tipoAgrotoxico == null)
            {
                return NotFound();
            }

            _context.Tipo_Agrotoxicos.Remove(tipoAgrotoxico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoAgrotoxicoExists(int id)
        {
            return _context.Tipo_Agrotoxicos.Any(e => e.Id == id);
        }
    }
}
