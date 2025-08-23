using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using API_TCC.DTOs;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    public class LavourasController : BaseController
    {
        private readonly Contexto _context;

        public LavourasController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Lavouras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LavouraResponseDto>>> GetLavouras()
        {
            var usuarioId = GetUsuarioId();
            var lavouras = await _context.Lavouras
                .Where(l => l.UsuarioId == usuarioId)
                .ToListAsync();

            return lavouras.Select(l => new LavouraResponseDto
            {
                Id = l.Id,
                nome = l.nome,
                area = l.area,
                latitude = l.latitude,
                longitude = l.longitude
            }).ToList();
        }

        // GET: api/Lavouras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LavouraResponseDto>> GetLavoura(int id)
        {
            var usuarioId = GetUsuarioId();
            var lavoura = await _context.Lavouras
                .FirstOrDefaultAsync(l => l.Id == id && l.UsuarioId == usuarioId);

            if (lavoura == null)
            {
                return NotFound();
            }

            return new LavouraResponseDto
            {
                Id = lavoura.Id,
                nome = lavoura.nome,
                area = lavoura.area,
                latitude = lavoura.latitude,
                longitude = lavoura.longitude
            };
        }

        // GET: api/Lavouras/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<LavouraResponseDto>>> GetLavouraByNome(string nome)
        {
            var usuarioId = GetUsuarioId();
            var lavouras = await _context.Lavouras
                .Where(l => l.nome.Contains(nome) && l.UsuarioId == usuarioId)
                .ToListAsync();

            return lavouras.Select(l => new LavouraResponseDto
            {
                Id = l.Id,
                nome = l.nome,
                area = l.area,
                latitude = l.latitude,
                longitude = l.longitude
            }).ToList();
        }

        // PUT: api/Lavouras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLavoura(int id, LavouraDto lavouraDto)
        {
            var usuarioId = GetUsuarioId();
            var lavouraExistente = await _context.Lavouras
                .FirstOrDefaultAsync(l => l.Id == id && l.UsuarioId == usuarioId);

            if (lavouraExistente == null)
            {
                return NotFound();
            }

            lavouraExistente.nome = lavouraDto.nome;
            lavouraExistente.area = lavouraDto.area;
            lavouraExistente.latitude = lavouraDto.latitude;
            lavouraExistente.longitude = lavouraDto.longitude;

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
        [HttpPost]
        public async Task<ActionResult<LavouraResponseDto>> PostLavoura(LavouraDto lavouraDto)
        {
            var usuarioId = GetUsuarioId();

            var lavoura = new Lavoura
            {
                UsuarioId = usuarioId,
                nome = lavouraDto.nome,
                area = lavouraDto.area,
                latitude = lavouraDto.latitude,
                longitude = lavouraDto.longitude
            };

            _context.Lavouras.Add(lavoura);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLavoura), new { id = lavoura.Id }, new LavouraResponseDto
            {
                Id = lavoura.Id,
                nome = lavoura.nome,
                area = lavoura.area,
                latitude = lavoura.latitude,
                longitude = lavoura.longitude
            });
        }

        // DELETE: api/Lavouras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLavoura(int id)
        {
            var usuarioId = GetUsuarioId();
            var lavoura = await _context.Lavouras
                .Where(l => l.Id == id && l.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
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
