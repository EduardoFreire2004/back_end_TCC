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
    public class FornecedorInsumosController : ControllerBase
    {
        private readonly Contexto _context;

        public FornecedorInsumosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/FornecedorInsumos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorInsumo>>> GetFornecedores_Insumos()
        {
            return await _context.Fornecedores_Insumos.ToListAsync();
        }

        // GET: api/FornecedorInsumos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorInsumo>> GetFornecedorInsumo(int id)
        {
            var fornecedorInsumo = await _context.Fornecedores_Insumos.FindAsync(id);

            if (fornecedorInsumo == null)
            {
                return NotFound();
            }

            return fornecedorInsumo;
        }

        // PUT: api/FornecedorInsumos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedorInsumo(int id, FornecedorInsumo fornecedorInsumo)
        {
            if (id != fornecedorInsumo.Id)
            {
                return BadRequest();
            }

            _context.Entry(fornecedorInsumo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorInsumoExists(id))
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

        // POST: api/FornecedorInsumos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FornecedorInsumo>> PostFornecedorInsumo(FornecedorInsumo fornecedorInsumo)
        {
            _context.Fornecedores_Insumos.Add(fornecedorInsumo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFornecedorInsumo", new { id = fornecedorInsumo.Id }, fornecedorInsumo);
        }

        // DELETE: api/FornecedorInsumos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedorInsumo(int id)
        {
            var fornecedorInsumo = await _context.Fornecedores_Insumos.FindAsync(id);
            if (fornecedorInsumo == null)
            {
                return NotFound();
            }

            _context.Fornecedores_Insumos.Remove(fornecedorInsumo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FornecedorInsumoExists(int id)
        {
            return _context.Fornecedores_Insumos.Any(e => e.Id == id);
        }
    }
}
