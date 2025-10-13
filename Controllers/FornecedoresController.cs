using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using API_TCC.DTOs;
using API_TCC.DTOs.API_TCC.DTOs;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : BaseController
    {
        private readonly Contexto _context;

        public FornecedoresController(Contexto context)
        {
            _context = context;
        }

        // ✅ GET: api/Fornecedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorDTO>>> GetFornecedores()
        {
            var usuarioId = GetUsuarioId();

            var fornecedores = await _context.Fornecedores
                .Where(f => f.UsuarioId == usuarioId)
                .Select(f => new FornecedorDTO
                {
                    Id = f.Id,
                    Nome = f.nome,
                    Cnpj = f.cnpj,
                    Telefone = f.telefone
                })
                .ToListAsync();

            return Ok(fornecedores);
        }

        // ✅ GET: api/Fornecedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorDTO>> GetFornecedor(int id)
        {
            var usuarioId = GetUsuarioId();

            var fornecedor = await _context.Fornecedores
                .Where(f => f.Id == id && f.UsuarioId == usuarioId)
                .Select(f => new FornecedorDTO
                {
                    Id = f.Id,
                    Nome = f.nome,
                    Cnpj = f.cnpj,
                    Telefone = f.telefone
                })
                .FirstOrDefaultAsync();

            if (fornecedor == null)
                return NotFound();

            return Ok(fornecedor);
        }

        // GET: api/Fornecedores/buscar?tipo=nome&valor=xxx
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> BuscarFornecedor([FromQuery] string tipo, [FromQuery] string valor)
        {
            var usuarioId = GetUsuarioId();

            if (string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(valor))
                return BadRequest("Tipo e valor são obrigatórios.");

            IQueryable<Fornecedor> query = _context.Fornecedores
                .Where(f => f.UsuarioId == usuarioId);

            switch (tipo.ToLower())
            {
                case "nome":
                    query = query.Where(f => f.nome.Contains(valor));
                    break;
                case "cnpj":
                    query = query.Where(f => f.cnpj.Contains(valor));
                    break;
                case "telefone":
                    query = query.Where(f => f.telefone.Contains(valor));
                    break;
                default:
                    return BadRequest("Tipo de busca inválido. Use: nome, cnpj ou telefone.");
            }

            var fornecedores = await query.ToListAsync();

            if (fornecedores == null || fornecedores.Count == 0)
                return NotFound("Nenhum fornecedor encontrado.");

            return fornecedores;
        }


        // ✅ GET: api/Fornecedores/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<FornecedorDTO>>> GetFornecedorByNome(string nome)
        {
            var usuarioId = GetUsuarioId();

            var fornecedores = await _context.Fornecedores
                .Where(f => f.nome.Contains(nome) && f.UsuarioId == usuarioId)
                .Select(f => new FornecedorDTO
                {
                    Id = f.Id,
                    Nome = f.nome,
                    Cnpj = f.cnpj,
                    Telefone = f.telefone
                })
                .ToListAsync();

            if (!fornecedores.Any())
                return NotFound();

            return Ok(fornecedores);
        }

        // ✅ GET: api/Fornecedores/cnpj/{cnpj}
        [HttpGet("cnpj/{cnpj}")]
        public async Task<ActionResult<FornecedorDTO>> GetFornecedorByCnpj(string cnpj)
        {
            var usuarioId = GetUsuarioId();

            var fornecedor = await _context.Fornecedores
                .Where(f => f.cnpj == cnpj && f.UsuarioId == usuarioId)
                .Select(f => new FornecedorDTO
                {
                    Id = f.Id,
                    Nome = f.nome,
                    Cnpj = f.cnpj,
                    Telefone = f.telefone
                })
                .FirstOrDefaultAsync();

            if (fornecedor == null)
                return NotFound();

            return Ok(fornecedor);
        }

        // ✅ POST: api/Fornecedores
        [HttpPost]
        public async Task<ActionResult<FornecedorDTO>> PostFornecedor(FornecedorDTO dto)
        {
            var usuarioId = GetUsuarioId();

            var fornecedor = new Fornecedor
            {
                UsuarioId = usuarioId,
                nome = dto.Nome,
                cnpj = dto.Cnpj,
                telefone = dto.Telefone
            };

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            dto.Id = fornecedor.Id;

            return CreatedAtAction(nameof(GetFornecedor), new { id = dto.Id }, dto);
        }

        // ✅ PUT: api/Fornecedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedor(int id, FornecedorDTO dto)
        {
            var usuarioId = GetUsuarioId();

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (fornecedor == null)
                return NotFound();

            fornecedor.nome = dto.Nome;
            fornecedor.cnpj = dto.Cnpj;
            fornecedor.telefone = dto.Telefone;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/Fornecedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedor(int id)
        {
            var usuarioId = GetUsuarioId();

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (fornecedor == null)
                return NotFound();

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
