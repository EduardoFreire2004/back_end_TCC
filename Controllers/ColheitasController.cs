using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColheitasController : BaseController
    {
        private readonly Contexto _context;

        public ColheitasController(Contexto context)
        {
            _context = context;
        }

        // ✅ GET: api/Colheitas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetColheitas()
        {
            var usuarioId = GetUsuarioId();

            var colheitas = await _context.Colheitas
                .Where(c => c.UsuarioId == usuarioId)
                .Select(c => new
                {
                    c.Id,
                    c.lavouraID,
                    c.tipo,
                    c.dataHora,
                    c.descricao,
                    c.quantidadeSacas,
                    c.areaHectares,
                    c.cooperativaDestino,
                    c.precoPorSaca
                })
                .ToListAsync();

            return Ok(colheitas);
        }

        // ✅ GET: api/Colheitas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetColheita(int id)
        {
            var usuarioId = GetUsuarioId();

            var colheita = await _context.Colheitas
                .Where(c => c.Id == id && c.UsuarioId == usuarioId)
                .Select(c => new
                {
                    c.Id,
                    c.lavouraID,
                    c.tipo,
                    c.dataHora,
                    c.descricao,
                    c.quantidadeSacas,
                    c.areaHectares,
                    c.cooperativaDestino,
                    c.precoPorSaca
                })
                .FirstOrDefaultAsync();

            if (colheita == null)
                return NotFound();

            return Ok(colheita);
        }

        // ✅ GET: api/Colheitas/porlavoura/{lavouraId}
        [HttpGet("porlavoura/{lavouraId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetColheitaPorLavoura(int lavouraId)
        {
            var usuarioId = GetUsuarioId();

            var colheitas = await _context.Colheitas
                .Where(c => c.lavouraID == lavouraId && c.UsuarioId == usuarioId)
                .Select(c => new
                {
                    c.Id,
                    c.lavouraID,
                    c.tipo,
                    c.dataHora,
                    c.descricao,
                    c.quantidadeSacas,
                    c.areaHectares,
                    c.cooperativaDestino,
                    c.precoPorSaca
                })
                .ToListAsync();

            return Ok(colheitas);
        }

        // ✅ GET: api/Colheitas/porlavoura/{lavouraId}/buscar?nome=...
        [HttpGet("porlavoura/{lavouraId}/buscar")]
        public async Task<ActionResult<IEnumerable<object>>> GetByNome(int lavouraId, string nome)
        {
            var usuarioId = GetUsuarioId();

            var colheitas = await _context.Colheitas
                .Where(c => c.lavouraID == lavouraId &&
                            c.UsuarioId == usuarioId &&
                            (c.descricao.Contains(nome) || c.tipo.Contains(nome)))
                .Select(c => new
                {
                    c.Id,
                    c.lavouraID,
                    c.tipo,
                    c.dataHora,
                    c.descricao,
                    c.quantidadeSacas,
                    c.areaHectares,
                    c.cooperativaDestino,
                    c.precoPorSaca
                })
                .ToListAsync();

            return Ok(colheitas);
        }

        // ✅ PUT: api/Colheitas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColheita(int id, Colheita colheita)
        {
            var usuarioId = GetUsuarioId();

            if (id != colheita.Id)
                return BadRequest("ID da colheita não corresponde.");

            var colheitaExistente = await _context.Colheitas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (colheitaExistente == null)
                return NotFound();

            colheita.UsuarioId = usuarioId;
            _context.Entry(colheitaExistente).CurrentValues.SetValues(colheita);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ POST: api/Colheitas
        [HttpPost]
        public async Task<ActionResult<object>> PostColheita(Colheita colheita)
        {
            var usuarioId = GetUsuarioId();
            colheita.UsuarioId = usuarioId;

            _context.Colheitas.Add(colheita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColheita), new { id = colheita.Id }, new
            {
                colheita.Id,
                colheita.lavouraID,
                colheita.tipo,
                colheita.dataHora,
                colheita.descricao,
                colheita.quantidadeSacas,
                colheita.areaHectares,
                colheita.cooperativaDestino,
                colheita.precoPorSaca
            });
        }

        // ✅ DELETE: api/Colheitas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColheita(int id)
        {
            var usuarioId = GetUsuarioId();

            var colheita = await _context.Colheitas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (colheita == null)
                return NotFound();

            _context.Colheitas.Remove(colheita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ GET: api/Colheitas/rendimento/lavoura/{lavouraId}
        [HttpGet("rendimento/lavoura/{lavouraId}")]
        public async Task<ActionResult<object>> GetRendimentoPorHectare(int lavouraId)
        {
            var usuarioId = GetUsuarioId();

            var colheitas = await _context.Colheitas
                .Where(c => c.lavouraID == lavouraId && c.UsuarioId == usuarioId)
                .ToListAsync();

            if (!colheitas.Any())
                return NotFound("Nenhuma colheita encontrada para essa lavoura.");

            var totalSacas = colheitas.Sum(c => c.quantidadeSacas);
            var totalArea = colheitas.Sum(c => c.areaHectares);
            var totalReceita = colheitas.Sum(c => c.quantidadeSacas * c.precoPorSaca);

            if (totalArea == 0)
                return BadRequest("Área total igual a zero. Verifique os dados cadastrados.");

            var rendimento = totalSacas / totalArea;

            return Ok(new
            {
                lavouraId,
                rendimentoPorHectare = rendimento,
                totalSacas,
                totalArea,
                totalReceita
            });
        }

        // ✅ GET: api/Colheitas/estatisticas/lavoura/{lavouraId}
        [HttpGet("estatisticas/lavoura/{lavouraId}")]
        public async Task<ActionResult<object>> GetEstatisticasColheita(int lavouraId)
        {
            var usuarioId = GetUsuarioId();

            var colheitas = await _context.Colheitas
                .Where(c => c.lavouraID == lavouraId && c.UsuarioId == usuarioId)
                .ToListAsync();

            if (!colheitas.Any())
                return NotFound(new { message = "Nenhuma colheita encontrada para a lavoura." });

            var totalSacas = colheitas.Sum(c => c.quantidadeSacas);
            var totalValor = colheitas.Sum(c => c.quantidadeSacas * c.precoPorSaca);
            var mediaSacas = totalSacas / colheitas.Count;
            var mediaValor = totalValor / colheitas.Count;

            var melhorColheita = colheitas.OrderByDescending(c => c.quantidadeSacas).First().dataHora;
            var piorColheita = colheitas.OrderBy(c => c.quantidadeSacas).First().dataHora;

            return Ok(new
            {
                totalColheitas = colheitas.Count,
                totalSacas,
                mediaSacas,
                totalValor,
                mediaValor,
                mediaRendimento = colheitas.Average(c => c.quantidadeSacas / c.areaHectares),
                melhorColheita,
                piorColheita
            });
        }

        private bool ColheitaExists(int id, int usuarioId)
        {
            return _context.Colheitas.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}
