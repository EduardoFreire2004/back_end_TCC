using System;
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
    public class InsumosController : BaseController
    {
        private readonly Contexto _context;

        public InsumosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Insumos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsumoDTO>>> GetInsumos()
        {
            var usuarioId = GetUsuarioId();

            var insumos = await _context.Insumos
                .Where(i => i.UsuarioId == usuarioId)
                .Select(i => new InsumoDTO
                {
                    Id = i.Id,
                    CategoriaInsumoID = i.categoriaInsumoID,
                    FornecedorInsumoID = i.fornecedorID,
                    Nome = i.nome,
                    Unidade_Medida = i.unidade_Medida,
                    Data_Cadastro = i.data_Cadastro,
                    Qtde = i.qtde,
                    Preco = i.preco
                })
                .ToListAsync();

            return Ok(insumos);
        }

        // GET: api/Insumos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InsumoDTO>> GetInsumo(int id)
        {
            var usuarioId = GetUsuarioId();

            var insumo = await _context.Insumos
                .Where(i => i.Id == id && i.UsuarioId == usuarioId)
                .Select(i => new InsumoDTO
                {
                    Id = i.Id,
                    CategoriaInsumoID = i.categoriaInsumoID,
                    FornecedorInsumoID = i.fornecedorID,
                    Nome = i.nome,
                    Unidade_Medida = i.unidade_Medida,
                    Data_Cadastro = i.data_Cadastro,
                    Qtde = i.qtde,
                    Preco = i.preco
                })
                .FirstOrDefaultAsync();

            if (insumo == null)
                return NotFound();

            return Ok(insumo);
        }

        // GET: api/Insumos/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<InsumoDTO>>> GetInsumoByNome(string nome)
        {
            var usuarioId = GetUsuarioId();

            var insumos = await _context.Insumos
                .Where(i => i.nome.Contains(nome) && i.UsuarioId == usuarioId)
                .Select(i => new InsumoDTO
                {
                    Id = i.Id,
                    CategoriaInsumoID = i.categoriaInsumoID,
                    FornecedorInsumoID = i.fornecedorID,
                    Nome = i.nome,
                    Unidade_Medida = i.unidade_Medida,
                    Data_Cadastro = i.data_Cadastro,
                    Qtde = i.qtde,
                    Preco = i.preco
                })
                .ToListAsync();

            if (insumos == null || insumos.Count == 0)
                return NotFound();

            return Ok(insumos);
        }

        // POST: api/Insumos
        [HttpPost]
        public async Task<ActionResult<InsumoDTO>> PostInsumo(InsumoDTO dto)
        {
            var usuarioId = GetUsuarioId();

            var insumo = new Insumo
            {
                UsuarioId = usuarioId,
                categoriaInsumoID = dto.CategoriaInsumoID,
                fornecedorID = dto.FornecedorInsumoID,
                nome = dto.Nome,
                unidade_Medida = dto.Unidade_Medida,
                data_Cadastro = dto.Data_Cadastro,
                qtde = (float)dto.Qtde,
                preco = (float)dto.Preco
            };

            _context.Insumos.Add(insumo);
            await _context.SaveChangesAsync();

            dto.Id = insumo.Id; // retornar o id gerado

            return CreatedAtAction(nameof(GetInsumo), new { id = dto.Id }, dto);
        }

        // PUT: api/Insumos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsumo(int id, InsumoDTO dto)
        {
            var usuarioId = GetUsuarioId();

            var insumo = await _context.Insumos
                .FirstOrDefaultAsync(i => i.Id == id && i.UsuarioId == usuarioId);

            if (insumo == null)
                return NotFound();

            insumo.categoriaInsumoID = dto.CategoriaInsumoID;
            insumo.fornecedorID = dto.FornecedorInsumoID;
            insumo.nome = dto.Nome;
            insumo.unidade_Medida = dto.Unidade_Medida;
            insumo.data_Cadastro = dto.Data_Cadastro;
            insumo.qtde = (float)dto.Qtde;
            insumo.preco = (float)dto.Preco;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Insumos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsumo(int id)
        {
            var usuarioId = GetUsuarioId();

            var insumo = await _context.Insumos
                .FirstOrDefaultAsync(i => i.Id == id && i.UsuarioId == usuarioId);

            if (insumo == null)
                return NotFound();

            _context.Insumos.Remove(insumo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
