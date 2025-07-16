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
    public class FornecedorAgrotoxicosController : ControllerBase
    {
        private readonly Contexto _context;

        public FornecedorAgrotoxicosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/FornecedorAgrotoxicos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorAgrotoxico>>> GetFornecedores_Agrotoxico()
        {
            return await _context.Fornecedores_Agrotoxico.ToListAsync();
        }

        // GET: api/FornecedorAgrotoxicos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorAgrotoxico>> GetFornecedorAgrotoxico(int id)
        {
            var fornecedorAgrotoxico = await _context.Fornecedores_Agrotoxico.FindAsync(id);

            if (fornecedorAgrotoxico == null)
            {
                return NotFound();
            }

            return fornecedorAgrotoxico;
        }

        // PUT: api/FornecedorAgrotoxicos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedorAgrotoxico(int id, FornecedorAgrotoxico fornecedorAgrotoxico)
        {
            if (id != fornecedorAgrotoxico.Id)
            {
                return BadRequest();
            }

            _context.Entry(fornecedorAgrotoxico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorAgrotoxicoExists(id))
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

        // POST: api/FornecedorAgrotoxicos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FornecedorAgrotoxico>> PostFornecedorAgrotoxico(FornecedorAgrotoxico fornecedorAgrotoxico)
        {
            _context.Fornecedores_Agrotoxico.Add(fornecedorAgrotoxico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFornecedorAgrotoxico", new { id = fornecedorAgrotoxico.Id }, fornecedorAgrotoxico);
        }

        // DELETE: api/FornecedorAgrotoxicos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedorAgrotoxico(int id)
        {
            var fornecedorAgrotoxico = await _context.Fornecedores_Agrotoxico.FindAsync(id);
            if (fornecedorAgrotoxico == null)
            {
                return NotFound();
            }

            _context.Fornecedores_Agrotoxico.Remove(fornecedorAgrotoxico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FornecedorAgrotoxicoExists(int id)
        {
            return _context.Fornecedores_Agrotoxico.Any(e => e.Id == id);
        }
    }
}
