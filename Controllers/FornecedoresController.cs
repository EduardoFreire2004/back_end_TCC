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
    public class FornecedoresController : BaseController
    {
        private readonly Contexto _context;

        public FornecedoresController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Fornecedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorResponseDto>>> GetFornecedores()
        {
            var usuarioId = GetUsuarioId();
            var fornecedores = await _context.Fornecedores
                .Where(f => f.UsuarioId == usuarioId)
                .ToListAsync();

            return fornecedores.Select(f => new FornecedorResponseDto
            {
                Id = f.Id,
                nome = f.nome,
                cnpj = f.cnpj,
                telefone = f.telefone
            }).ToList();
        }

        // GET: api/Fornecedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> GetFornecedor(int id)
        {
            var usuarioId = GetUsuarioId();
            var fornecedor = await _context.Fornecedores
                .Where(f => f.Id == id && f.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (fornecedor == null)
            {
                return NotFound();
            }

            return fornecedor;
        }

        // GET: api/Fornecedores/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedorByNome(string nome)
        {
            var usuarioId = GetUsuarioId();
            var fornecedores = await _context.Fornecedores
                .Where(f => f.nome.Contains(nome) && f.UsuarioId == usuarioId)
                .ToListAsync();

            if (fornecedores == null || fornecedores.Count == 0)
            {
                return NotFound();
            }

            return fornecedores;
        }

        // GET: api/Fornecedores/cnpj/{cnpj}
        [HttpGet("cnpj/{cnpj}")]
        public async Task<ActionResult<Fornecedor>> GetFornecedorByCnpj(string cnpj)
        {
            var usuarioId = GetUsuarioId();
            var fornecedor = await _context.Fornecedores
                .Where(f => f.cnpj == cnpj && f.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (fornecedor == null)
            {
                return NotFound();
            }

            return fornecedor;
        }

        // PUT: api/Fornecedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedor(int id, Fornecedor fornecedor)
        {
            var usuarioId = GetUsuarioId();
            
            if (id != fornecedor.Id)
            {
                return BadRequest();
            }

            // Verificar se o fornecedor pertence ao usuário
            var fornecedorExistente = await _context.Fornecedores
                .Where(f => f.Id == id && f.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (fornecedorExistente == null)
            {
                return NotFound();
            }

            // Garantir que o UsuarioId não seja alterado
            fornecedor.UsuarioId = usuarioId;

            _context.Entry(fornecedor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorExists(id, usuarioId))
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

        // POST: api/Fornecedores
        [HttpPost]
        public async Task<ActionResult<Fornecedor>> PostFornecedor(Fornecedor fornecedor)
        {
            var usuarioId = GetUsuarioId();
            fornecedor.UsuarioId = usuarioId;
            
            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFornecedor", new { id = fornecedor.Id }, fornecedor);
        }

        // DELETE: api/Fornecedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedor(int id)
        {
            var usuarioId = GetUsuarioId();
            var fornecedor = await _context.Fornecedores
                .Where(f => f.Id == id && f.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (fornecedor == null)
            {
                return NotFound();
            }

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FornecedorExists(int id, int usuarioId)
        {
            return _context.Fornecedores.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}

