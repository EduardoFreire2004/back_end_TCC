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
    public class PlantiosController : BaseController
    {
        private readonly Contexto _context;
        private readonly IEstoqueService _estoqueService;

        public PlantiosController(Contexto context, IEstoqueService estoqueService)
        {
            _context = context;
            _estoqueService = estoqueService;
        }

        // GET: api/Plantios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plantio>>> GetPlantios()
        {
            var usuarioId = GetUsuarioId();
            return await _context.Plantios
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // GET: api/Plantios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plantio>> GetPlantio(int id)
        {
            var usuarioId = GetUsuarioId();
            var plantio = await _context.Plantios
                .Where(p => p.Id == id && p.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (plantio == null)
            {
                return NotFound();
            }

            return plantio;
        }

        // GET: api/Plantios/porlavoura/5
        [HttpGet("porlavoura/{lavouraId}")]
        public async Task<ActionResult<IEnumerable<Plantio>>> GetPlantioPorLavoura(int lavouraId)
        {
            var usuarioId = GetUsuarioId();
            var plantios = await _context.Plantios
                .Where(p => p.lavouraID == lavouraId && p.UsuarioId == usuarioId)
                .ToListAsync();

            return plantios;
        }

        // PUT: api/Plantios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlantio(int id, Plantio plantio)
        {
            var usuarioId = GetUsuarioId();
            
            if (id != plantio.Id)
            {
                return BadRequest();
            }

            // Verificar se o plantio pertence ao usuário
            var plantioExistente = await _context.Plantios
                .Where(p => p.Id == id && p.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (plantioExistente == null)
            {
                return NotFound();
            }

            // Verificar se houve mudança na quantidade
            if (plantio.qtde != plantioExistente.qtde)
            {
                // Se a nova quantidade for maior, verificar se há estoque suficiente
                if (plantio.qtde > plantioExistente.qtde)
                {
                    var quantidadeAdicional = plantio.qtde - plantioExistente.qtde;
                    var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelSementeAsync(plantio.sementeID, usuarioId);
                    
                    if (estoqueDisponivel < quantidadeAdicional)
                    {
                        return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Necessário: {quantidadeAdicional}" });
                    }
                    
                    // Baixar estoque adicional
                    await _estoqueService.BaixarEstoqueSementeAsync(plantio.sementeID, quantidadeAdicional, usuarioId);
                }
                // Se a nova quantidade for menor, retornar diferença ao estoque
                else if (plantio.qtde < plantioExistente.qtde)
                {
                    var quantidadeRetornar = plantioExistente.qtde - plantio.qtde;
                    await _estoqueService.RetornarEstoqueSementeAsync(plantio.sementeID, quantidadeRetornar, usuarioId);
                }
            }

            // Garantir que o UsuarioId não seja alterado
            plantio.UsuarioId = usuarioId;

            _context.Entry(plantio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantioExists(id, usuarioId))
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
        [HttpPost]
        public async Task<ActionResult<Plantio>> PostPlantio(Plantio plantio)
        {
            var usuarioId = GetUsuarioId();
            plantio.UsuarioId = usuarioId;
            
            // Verificar se há estoque suficiente
            if (!await _estoqueService.VerificarEstoqueSementeAsync(plantio.sementeID, plantio.qtde, usuarioId))
            {
                var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelSementeAsync(plantio.sementeID, usuarioId);
                return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Solicitado: {plantio.qtde}" });
            }
            
            // Baixar estoque
            await _estoqueService.BaixarEstoqueSementeAsync(plantio.sementeID, plantio.qtde, usuarioId);
            
            _context.Plantios.Add(plantio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlantio", new { id = plantio.Id }, plantio);
        }

        // DELETE: api/Plantios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantio(int id)
        {
            var usuarioId = GetUsuarioId();
            var plantio = await _context.Plantios
                .Where(p => p.Id == id && p.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (plantio == null)
            {
                return NotFound();
            }

            // Retornar quantidade ao estoque
            await _estoqueService.RetornarEstoqueSementeAsync(plantio.sementeID, plantio.qtde, usuarioId);

            _context.Plantios.Remove(plantio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlantioExists(int id, int usuarioId)
        {
            return _context.Plantios.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}
