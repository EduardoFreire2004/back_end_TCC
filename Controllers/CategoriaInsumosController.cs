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
    public class CategoriaInsumosController : ControllerBase
    {
        private readonly Contexto _context;

        public CategoriaInsumosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/CategoriaInsumos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaInsumo>>> GetCategorias_Insumos()
        {
            return await _context.Categorias_Insumos.ToListAsync();
        }

        // GET: api/CategoriaInsumos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaInsumo>> GetCategoriaInsumo(int id)
        {
            var categoriaInsumo = await _context.Categorias_Insumos.FindAsync(id);

            if (categoriaInsumo == null)
            {
                return NotFound();
            }

            return categoriaInsumo;
        }

        // PUT: api/CategoriaInsumos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoriaInsumo(int id, CategoriaInsumo categoriaInsumo)
        {
            if (id != categoriaInsumo.Id)
            {
                return BadRequest();
            }

            _context.Entry(categoriaInsumo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaInsumoExists(id))
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

        // POST: api/CategoriaInsumos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoriaInsumo>> PostCategoriaInsumo(CategoriaInsumo categoriaInsumo)
        {
            _context.Categorias_Insumos.Add(categoriaInsumo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoriaInsumo", new { id = categoriaInsumo.Id }, categoriaInsumo);
        }

        // DELETE: api/CategoriaInsumos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaInsumo(int id)
        {
            var categoriaInsumo = await _context.Categorias_Insumos.FindAsync(id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }

            _context.Categorias_Insumos.Remove(categoriaInsumo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaInsumoExists(int id)
        {
            return _context.Categorias_Insumos.Any(e => e.Id == id);
        }
    }
}
