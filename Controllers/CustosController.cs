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
    public class CustosController : BaseController
    {
        private readonly Contexto _context;

        public CustosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Custos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Custo>>> GetCustos()
        {
            var usuarioId = GetUsuarioId();
            return await _context.Custo
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // GET: api/Custos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Custo>> GetCusto(int id)
        {
            var usuarioId = GetUsuarioId();
            var custo = await _context.Custo
                .Where(c => c.Id == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (custo == null)
            {
                return NotFound();
            }

            return custo;
        }

        // GET: api/Custos/lavoura/{lavouraId}
        [HttpGet("lavoura/{lavouraId}")]
        public async Task<ActionResult<IEnumerable<Custo>>> GetCustosByLavoura(int lavouraId)
        {
            var usuarioId = GetUsuarioId();
            var custos = await _context.Custo
                .Where(c => c.lavouraID == lavouraId && c.UsuarioId == usuarioId)
                .ToListAsync();

            if (custos == null || custos.Count == 0)
            {
                return NotFound();
            }

            return custos;
        }

        // PUT: api/Custos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCusto(int id, Custo custo)
        {
            var usuarioId = GetUsuarioId();
            
            if (id != custo.Id)
            {
                return BadRequest();
            }

            // Verificar se o custo pertence ao usuário
            var custoExistente = await _context.Custo
                .Where(c => c.Id == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (custoExistente == null)
            {
                return NotFound();
            }

            // Garantir que o UsuarioId não seja alterado
            custo.UsuarioId = usuarioId;

            _context.Entry(custo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustoExists(id, usuarioId))
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

        // POST: api/Custos
        [HttpPost]
        public async Task<ActionResult<Custo>> PostCusto(Custo custo)
        {
            var usuarioId = GetUsuarioId();
            custo.UsuarioId = usuarioId;
            
            _context.Custo.Add(custo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCusto", new { id = custo.Id }, custo);
        }

        // DELETE: api/Custos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCusto(int id)
        {
            var usuarioId = GetUsuarioId();
            var custo = await _context.Custo
                .Where(c => c.Id == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (custo == null)
            {
                return NotFound();
            }

            _context.Custo.Remove(custo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustoExists(int id, int usuarioId)
        {
            return _context.Custo.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }

        // PUT: api/Custos/calcular/3
        [HttpPut("calcular/{lavouraId}")]
        public async Task<IActionResult> CalcularCustosEGanhos(int lavouraId)
        {
            var custos = await _context.Custo
                .Where(c => c.lavouraID == lavouraId)
                .ToListAsync();

            if (!custos.Any())
            {
                var todosCustos = await _context.Custo.ToListAsync();
                Console.WriteLine($"Total de custos no banco: {todosCustos.Count}");
                foreach (var c in todosCustos)
                {
                    Console.WriteLine($"Custo ID: {c.Id}, lavouraID: {c.lavouraID}");
                }

                return NotFound("Nenhum custo encontrado para essa lavoura.");
            }


            foreach (var custo in custos)
            {
                decimal totalCusto = 0;
                decimal totalGanho = 0;

                // Agrotóxicos
                if (custo.aplicacaoAgrotoxicoID.HasValue)
                {
                    var aplicacaoAgro = await _context.Aplicacoes
                        .Include(a => a.agrotoxico)
                        .FirstOrDefaultAsync(a => a.Id == custo.aplicacaoAgrotoxicoID.Value);

                    if (aplicacaoAgro?.agrotoxico != null)
                    {
                        totalCusto += (decimal)(aplicacaoAgro.agrotoxico.qtde * aplicacaoAgro.agrotoxico.preco);
                    }
                }

                // Insumos
                if (custo.aplicacaoInsumoID.HasValue)
                {
                    var aplicacaoInsumo = await _context.Aplicacao_Insumos
                        .Include(ai => ai.insumo)
                        .FirstOrDefaultAsync(ai => ai.Id == custo.aplicacaoInsumoID.Value);

                    if (aplicacaoInsumo?.insumo != null)
                    {
                        totalCusto += (decimal)(aplicacaoInsumo.insumo.qtde * aplicacaoInsumo.insumo.preco);
                    }
                }

                // Plantios
                if (custo.plantioID.HasValue)
                {
                    var plantio = await _context.Plantios
                        .Include(p => p.semente)
                        .FirstOrDefaultAsync(p => p.Id == custo.plantioID.Value);

                    if (plantio?.semente != null)
                    {
                        totalCusto += (decimal)(plantio.semente.qtde * plantio.semente.preco);
                    }
                }

                // Colheitas (Ganhos)
                if (custo.colheitaID.HasValue)
                {
                    var colheita = await _context.Colheitas
                        .FirstOrDefaultAsync(c => c.Id == custo.colheitaID.Value);

                    if (colheita != null)
                    {
                        totalGanho += (decimal)(colheita.quantidadeSacas * colheita.precoPorSaca);
                    }
                }

                custo.custoTotal = (double)totalCusto;
                custo.ganhoTotal = (double)totalGanho;
            }

            await _context.SaveChangesAsync();

            return Ok("Custos e ganhos atualizados com sucesso.");
        }

    }
}
