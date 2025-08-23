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
    public class InsumosController : BaseController
    {
        private readonly Contexto _context;

        public InsumosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Insumos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsumoResponseDto>>> GetInsumos()
        {
            var usuarioId = GetUsuarioId();
            var insumos = await _context.Insumos
                .Where(i => i.UsuarioId == usuarioId)
                .Include(i => i.categoriaInsumo)
                .Include(i => i.fornecedor)
                .ToListAsync();

            return insumos.Select(i => new InsumoResponseDto
            {
                Id = i.Id,
                categoriaInsumoID = i.categoriaInsumoID,
                fornecedorInsumoID = i.fornecedorID,
                nome = i.nome,
                unidade_Medida = i.unidade_Medida,
                data_Cadastro = i.data_Cadastro,
                qtde = i.qtde,
                preco = i.preco,
                descricao = string.Empty, // O modelo Insumo não tem descricao
                CategoriaNome = i.categoriaInsumo?.descricao ?? string.Empty,
                FornecedorNome = i.fornecedor?.nome ?? string.Empty
            }).ToList();
        }

        // GET: api/Insumos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Insumo>> GetInsumo(int id)
        {
            var usuarioId = GetUsuarioId();
            var insumo = await _context.Insumos
                .Where(i => i.Id == id && i.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (insumo == null)
            {
                return NotFound();
            }

            return insumo;
        }

        // GET: api/Insumos/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Insumo>>> GetInsumoByNome(string nome)
        {
            var usuarioId = GetUsuarioId();
            var insumos = await _context.Insumos
                .Where(i => i.nome.Contains(nome) && i.UsuarioId == usuarioId)
                .ToListAsync();

            if (insumos == null || insumos.Count == 0)
            {
                return NotFound();
            }

            return insumos;
        }


        // PUT: api/Insumos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsumo(int id, Insumo insumo)
        {
            var usuarioId = GetUsuarioId();
            
            if (id != insumo.Id)
            {
                return BadRequest();
            }

            // Verificar se o insumo pertence ao usuário
            var insumoExistente = await _context.Insumos
                .Where(i => i.Id == id && i.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (insumoExistente == null)
            {
                return NotFound();
            }

            // Garantir que o UsuarioId não seja alterado
            insumo.UsuarioId = usuarioId;

            _context.Entry(insumo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsumoExists(id, usuarioId))
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

        // POST: api/Insumos
        [HttpPost]
        public async Task<ActionResult<Insumo>> PostInsumo(Insumo insumo)
        {
            var usuarioId = GetUsuarioId();
            insumo.UsuarioId = usuarioId;
            
            _context.Insumos.Add(insumo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInsumo", new { id = insumo.Id }, insumo);
        }

        // DELETE: api/Insumos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsumo(int id)
        {
            var usuarioId = GetUsuarioId();
            var insumo = await _context.Insumos
                .Where(i => i.Id == id && i.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (insumo == null)
            {
                return NotFound();
            }

            _context.Insumos.Remove(insumo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InsumoExists(int id, int usuarioId)
        {
            return _context.Insumos.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}
