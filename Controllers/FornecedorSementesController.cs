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
    public class FornecedorSementesController : ControllerBase
    {
        private readonly Contexto _context;

        public FornecedorSementesController(Contexto context)
        {
            _context = context;
        }

        // GET: api/FornecedorSementes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorSemente>>> GetFornedores_Sementes()
        {
            return await _context.Fornedores_Sementes.ToListAsync();
        }

        // GET: api/FornecedorSementes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorSemente>> GetFornecedorSemente(int id)
        {
            var fornecedorSemente = await _context.Fornedores_Sementes.FindAsync(id);

            if (fornecedorSemente == null)
            {
                return NotFound();
            }

            return fornecedorSemente;
        }

        // PUT: api/FornecedorSementes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedorSemente(int id, FornecedorSemente fornecedorSemente)
        {
            if (id != fornecedorSemente.Id)
            {
                return BadRequest();
            }

            _context.Entry(fornecedorSemente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorSementeExists(id))
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

        // POST: api/FornecedorSementes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FornecedorSemente>> PostFornecedorSemente(FornecedorSemente fornecedorSemente)
        {
            _context.Fornedores_Sementes.Add(fornecedorSemente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFornecedorSemente", new { id = fornecedorSemente.Id }, fornecedorSemente);
        }

        // DELETE: api/FornecedorSementes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedorSemente(int id)
        {
            var fornecedorSemente = await _context.Fornedores_Sementes.FindAsync(id);
            if (fornecedorSemente == null)
            {
                return NotFound();
            }

            _context.Fornedores_Sementes.Remove(fornecedorSemente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FornecedorSementeExists(int id)
        {
            return _context.Fornedores_Sementes.Any(e => e.Id == id);
        }
    }
}
