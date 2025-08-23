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
    public class SementesController : BaseController
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
            var usuarioId = GetUsuarioId();
            return await _context.Sementes
                .Where(s => s.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // GET: api/Sementes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Semente>> GetSemente(int id)
        {
            var usuarioId = GetUsuarioId();
            var semente = await _context.Sementes
                .Where(s => s.Id == id && s.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (semente == null)
            {
                return NotFound();
            }

            return semente;
        }

        // GET: api/Sementes/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Semente>>> GetSementeByNome(string nome)
        {
            var usuarioId = GetUsuarioId();
            var sementes = await _context.Sementes
                .Where(s => s.nome.Contains(nome) && s.UsuarioId == usuarioId)
                .ToListAsync();

            if (sementes == null || sementes.Count == 0)
            {
                return NotFound();
            }

            return sementes;
        }


        // PUT: api/Sementes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSemente(int id, Semente semente)
        {
            var usuarioId = GetUsuarioId();
            
            if (id != semente.Id)
            {
                return BadRequest();
            }

            // Verificar se a semente pertence ao usuário
            var sementeExistente = await _context.Sementes
                .Where(s => s.Id == id && s.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (sementeExistente == null)
            {
                return NotFound();
            }

            // Garantir que o UsuarioId não seja alterado
            semente.UsuarioId = usuarioId;

            _context.Entry(semente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SementeExists(id, usuarioId))
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
        [HttpPost]
        public async Task<ActionResult<Semente>> PostSemente(Semente semente)
        {
            var usuarioId = GetUsuarioId();
            semente.UsuarioId = usuarioId;
            
            _context.Sementes.Add(semente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSemente", new { id = semente.Id }, semente);
        }

        // DELETE: api/Sementes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemente(int id)
        {
            var usuarioId = GetUsuarioId();
            var semente = await _context.Sementes
                .Where(s => s.Id == id && s.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (semente == null)
            {
                return NotFound();
            }

            _context.Sementes.Remove(semente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SementeExists(int id, int usuarioId)
        {
            return _context.Sementes.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}
