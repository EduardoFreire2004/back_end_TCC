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
    public class ColheitasController : BaseController
    {
        private readonly Contexto _context;

        public ColheitasController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Colheitas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colheita>>> GetColheitas()
        {
            var usuarioId = GetUsuarioId();
            return await _context.Colheitas
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // GET: api/Colheitas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Colheita>> GetColheita(int id)
        {
            var usuarioId = GetUsuarioId();
            var colheita = await _context.Colheitas
                .Where(c => c.Id == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (colheita == null)
            {
                return NotFound();
            }

            return colheita;
        }



        // GET: api/Colheitas/porlavoura/5
        [HttpGet("porlavoura/{lavouraId}")]
        public async Task<ActionResult<IEnumerable<Colheita>>> GetColheitaPorLavoura(int lavouraId)
        {
            var usuarioId = GetUsuarioId();
            var colheitas = await _context.Colheitas
                .Where(c => c.lavouraID == lavouraId && c.UsuarioId == usuarioId)
                .ToListAsync();

            return colheitas;
        }

        // GET: api/Colheitas/porlavoura/{lavouraId}/buscar?nome={nome}
        [HttpGet("porlavoura/{lavouraId}/buscar")]
        public async Task<ActionResult<IEnumerable<Colheita>>> GetByNome(string nome, int lavouraId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                
                var colheitas = await _context.Colheitas
                    .Where(c => c.lavouraID == lavouraId && 
                               c.UsuarioId == usuarioId && 
                               (c.descricao.Contains(nome) || c.tipo.Contains(nome)))
                    .ToListAsync();

                return colheitas;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao buscar por {nome}: {ex.Message}" });
            }
        }


        // PUT: api/Colheitas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColheita(int id, Colheita colheita)
        {
            var usuarioId = GetUsuarioId();
            
            if (id != colheita.Id)
            {
                return BadRequest();
            }

            // Verificar se a colheita pertence ao usuário
            var colheitaExistente = await _context.Colheitas
                .Where(c => c.Id == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (colheitaExistente == null)
            {
                return NotFound();
            }

            // Garantir que o UsuarioId não seja alterado
            colheita.UsuarioId = usuarioId;

            _context.Entry(colheita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColheitaExists(id, usuarioId))
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

        // POST: api/Colheitas
        [HttpPost]
        public async Task<ActionResult<Colheita>> PostColheita(Colheita colheita)
        {
            var usuarioId = GetUsuarioId();
            colheita.UsuarioId = usuarioId;
            
            _context.Colheitas.Add(colheita);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetColheita", new { id = colheita.Id }, colheita);
        }

        // GET: api/Colheitas/rendimento/lavoura/5
        [HttpGet("rendimento/lavoura/{lavouraId}")]
        public async Task<ActionResult<object>> GetRendimentoPorHectare(int lavouraId)
        {
            var usuarioId = GetUsuarioId();
            var colheitas = await _context.Colheitas
                .Where(c => c.lavouraID == lavouraId && c.UsuarioId == usuarioId)
                .ToListAsync();

            if (colheitas == null || !colheitas.Any())
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


        // DELETE: api/Colheitas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColheita(int id)
        {
            var usuarioId = GetUsuarioId();
            var colheita = await _context.Colheitas
                .Where(c => c.Id == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (colheita == null)
            {
                return NotFound();
            }

            _context.Colheitas.Remove(colheita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColheitaExists(int id, int usuarioId)
        {
            return _context.Colheitas.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }

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

            // Exemplo simples para tendência e variação
            var tendencia = totalSacas > mediaSacas ? "crescente" : "estável";
            var variacao = (mediaSacas / totalSacas) * 100;

            return Ok(new
            {
                totalColheitas = colheitas.Count,
                totalSacas,
                mediaSacas,
                totalValor,
                mediaValor,
                mediaRendimento = colheitas.Average(c => c.quantidadeSacas / c.areaHectares),
                melhorColheita,
                piorColheita,
                tendencia,
                variacaoPeriodo = variacao
            });
        }
        
    }
}
