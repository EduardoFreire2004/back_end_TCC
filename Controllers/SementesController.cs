using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using API_TCC.DTOs;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using API_TCC.DTOs.API_TCC.DTOs;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SementesController : BaseController
    {
        private readonly Contexto _context;

        public SementesController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Sementes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SementeDTO>>> GetSementes()
        {
            var usuarioId = GetUsuarioId();

            var sementes = await _context.Sementes
                .Where(s => s.UsuarioId == usuarioId)
                .Select(s => new SementeDTO
                {
                    Id = s.Id,
                    FornecedorID = s.fornecedorID,
                    Nome = s.nome,
                    Tipo = s.tipo,
                    Marca = s.marca,
                    Qtde = s.qtde,
                    Preco = s.preco,
                    Data_Cadastro = s.data_Cadastro
                })
                .ToListAsync();

            return Ok(sementes);
        }

        // GET: api/Sementes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SementeDTO>> GetSemente(int id)
        {
            var usuarioId = GetUsuarioId();

            var s = await _context.Sementes
                .Where(x => x.Id == id && x.UsuarioId == usuarioId)
                .Select(x => new SementeDTO
                {
                    Id = x.Id,
                    FornecedorID = x.fornecedorID,
                    Nome = x.nome,
                    Tipo = x.tipo,
                    Marca = x.marca,
                    Qtde = x.qtde,
                    Preco = x.preco,
                    Data_Cadastro = x.data_Cadastro
                })
                .FirstOrDefaultAsync();

            if (s == null)
                return NotFound();

            return Ok(s);
        }

        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<SementeDTO>>> GetSementeByNome(string nome)
        {
            var usuarioId = GetUsuarioId();

            var sementes = await _context.Sementes
                .Where(i => i.nome.Contains(nome) && i.UsuarioId == usuarioId)
                .Select(i => new SementeDTO
                {
                    Id = i.Id,
                    FornecedorID = i.fornecedorID,
                    Nome = i.nome,
                    Tipo = i.tipo,
                    Marca = i.marca,
                    Qtde = i.qtde,
                    Preco = i.preco,
                    Data_Cadastro = i.data_Cadastro
                })
                .ToListAsync();

            if (sementes == null || sementes.Count == 0)
                return NotFound();

            return Ok(sementes);
        }

        // POST: api/Sementes
        [HttpPost]
        public async Task<ActionResult<Semente>> PostSemente(SementeDTO dto)
        {
            var usuarioId = GetUsuarioId();

            var semente = new Semente
            {
                UsuarioId = usuarioId,
                fornecedorID = dto.FornecedorID,
                nome = dto.Nome,
                tipo = dto.Tipo,
                marca = dto.Marca,
                qtde = (float)dto.Qtde,
                preco = (float)dto.Preco,
                data_Cadastro = dto.Data_Cadastro
            };

            _context.Sementes.Add(semente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSemente), new { id = semente.Id }, dto);
        }

        // PUT: api/Sementes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSemente(int id, SementeDTO dto)
        {
            var usuarioId = GetUsuarioId();

            var semente = await _context.Sementes
                .FirstOrDefaultAsync(s => s.Id == id && s.UsuarioId == usuarioId);

            if (semente == null)
                return NotFound();

            semente.fornecedorID = dto.FornecedorID;
            semente.nome = dto.Nome;
            semente.tipo = dto.Tipo;
            semente.marca = dto.Marca;
            semente.qtde = (float)dto.Qtde;
            semente.preco = (float)dto.Preco;
            semente.data_Cadastro = dto.Data_Cadastro;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Sementes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemente(int id)
        {
            var usuarioId = GetUsuarioId();

            var semente = await _context.Sementes
                .FirstOrDefaultAsync(s => s.Id == id && s.UsuarioId == usuarioId);

            if (semente == null)
                return NotFound();

            _context.Sementes.Remove(semente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
