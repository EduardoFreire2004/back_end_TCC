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
    public class PlantiosController : ControllerBase
    {
        private readonly Contexto _context;

        public PlantiosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Plantios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plantio>>> GetPlantios()
        {
            return await _context.Plantios.ToListAsync();
        }

        // GET: api/Plantios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plantio>> GetPlantio(int id)
        {
            var plantio = await _context.Plantios.FindAsync(id);

            if (plantio == null)
            {
                return NotFound();
            }

            return plantio;
        }

        // PUT: api/Plantios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlantio(int id, Plantio plantio)
        {
            if (id != plantio.Id)
            {
                return BadRequest();
            }

            _context.Entry(plantio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantioExists(id))
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

        // POST: api/Plantios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Plantio>> PostPlantio(Plantio plantio)
        {
            _context.Plantios.Add(plantio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlantio", new { id = plantio.Id }, plantio);
        }

        // DELETE: api/Plantios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantio(int id)
        {
            var plantio = await _context.Plantios.FindAsync(id);
            if (plantio == null)
            {
                return NotFound();
            }

            _context.Plantios.Remove(plantio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlantioExists(int id)
        {
            return _context.Plantios.Any(e => e.Id == id);
        }
    }
}
