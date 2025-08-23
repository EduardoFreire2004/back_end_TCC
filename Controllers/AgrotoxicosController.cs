using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using API_TCC.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    public class AgrotoxicosController : BaseController
    {
        private readonly Contexto _context;

        public AgrotoxicosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Agrotoxicos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgrotoxicoResponseDto>>> GetAgrotoxicos()
        {
            var usuarioId = GetUsuarioId();
            var agrotoxicos = await _context.Agrotoxicos
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.fornecedor)
                .Include(a => a.tipo)
                .ToListAsync();

            return agrotoxicos.Select(a => new AgrotoxicoResponseDto
            {
                Id = a.Id,
                fornecedorID = a.fornecedorID,
                tipoID = a.tipoID,
                nome = a.nome,
                unidade_Medida = a.unidade_Medida,
                data_Cadastro = a.data_Cadastro,
                qtde = a.qtde,
                preco = a.preco,
                FornecedorNome = a.fornecedor?.nome ?? string.Empty,
                TipoNome = a.tipo?.descricao ?? string.Empty
            }).ToList();
        }

        // GET: api/Agrotoxicos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgrotoxicoResponseDto>> GetAgrotoxico(int id)
        {
            var usuarioId = GetUsuarioId();
            var agrotoxico = await _context.Agrotoxicos
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .Include(a => a.fornecedor)
                .Include(a => a.tipo)
                .FirstOrDefaultAsync();

            if (agrotoxico == null)
            {
                return NotFound();
            }

            return new AgrotoxicoResponseDto
            {
                Id = agrotoxico.Id,
                fornecedorID = agrotoxico.fornecedorID,
                tipoID = agrotoxico.tipoID,
                nome = agrotoxico.nome,
                unidade_Medida = agrotoxico.unidade_Medida,
                data_Cadastro = agrotoxico.data_Cadastro,
                qtde = agrotoxico.qtde,
                preco = agrotoxico.preco,
                FornecedorNome = agrotoxico.fornecedor?.nome ?? string.Empty,
                TipoNome = agrotoxico.tipo?.descricao ?? string.Empty
            };
        }

        // GET: api/Agrotoxicos/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<AgrotoxicoResponseDto>>> GetAgrotoxicoByNome(string nome)
        {
            var usuarioId = GetUsuarioId();
            var agrotoxicos = await _context.Agrotoxicos
                .Where(a => a.nome.Contains(nome) && a.UsuarioId == usuarioId)
                .Include(a => a.fornecedor)
                .Include(a => a.tipo)
                .ToListAsync();

            if (agrotoxicos == null || agrotoxicos.Count == 0)
            {
                return NotFound();
            }

            return agrotoxicos.Select(a => new AgrotoxicoResponseDto
            {
                Id = a.Id,
                fornecedorID = a.fornecedorID,
                tipoID = a.tipoID,
                nome = a.nome,
                unidade_Medida = a.unidade_Medida,
                data_Cadastro = a.data_Cadastro,
                qtde = a.qtde,
                preco = a.preco,
                FornecedorNome = a.fornecedor?.nome ?? string.Empty,
                TipoNome = a.tipo?.descricao ?? string.Empty
            }).ToList();
        }

        // PUT: api/Agrotoxicos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgrotoxico(int id, AgrotoxicoDto agrotoxicoDto)
        {
            var usuarioId = GetUsuarioId();
            var agrotoxicoExistente = await _context.Agrotoxicos
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == usuarioId);

            if (agrotoxicoExistente == null)
            {
                return NotFound();
            }

            agrotoxicoExistente.fornecedorID = agrotoxicoDto.fornecedorID;
            agrotoxicoExistente.tipoID = agrotoxicoDto.tipoID;
            agrotoxicoExistente.nome = agrotoxicoDto.nome;
            agrotoxicoExistente.unidade_Medida = agrotoxicoDto.unidade_Medida;
            agrotoxicoExistente.data_Cadastro = agrotoxicoDto.data_Cadastro;
            agrotoxicoExistente.qtde = agrotoxicoDto.qtde;
            agrotoxicoExistente.preco = agrotoxicoDto.preco;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgrotoxicoExists(id))
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

        // POST: api/Agrotoxicos
        [HttpPost]
        public async Task<ActionResult<AgrotoxicoResponseDto>> PostAgrotoxico(AgrotoxicoDto agrotoxicoDto)
        {
            var usuarioId = GetUsuarioId();

            var agrotoxico = new Agrotoxico
            {
                UsuarioId = usuarioId,
                fornecedorID = agrotoxicoDto.fornecedorID,
                tipoID = agrotoxicoDto.tipoID,
                nome = agrotoxicoDto.nome,
                unidade_Medida = agrotoxicoDto.unidade_Medida,
                data_Cadastro = agrotoxicoDto.data_Cadastro,
                qtde = agrotoxicoDto.qtde,
                preco = agrotoxicoDto.preco
            };

            _context.Agrotoxicos.Add(agrotoxico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgrotoxico), new { id = agrotoxico.Id }, new AgrotoxicoResponseDto
            {
                Id = agrotoxico.Id,
                fornecedorID = agrotoxico.fornecedorID,
                tipoID = agrotoxico.tipoID,
                nome = agrotoxico.nome,
                unidade_Medida = agrotoxico.unidade_Medida,
                data_Cadastro = agrotoxico.data_Cadastro,
                qtde = agrotoxico.qtde,
                preco = agrotoxico.preco,
                FornecedorNome = string.Empty,
                TipoNome = string.Empty
            });
        }

        // DELETE: api/Agrotoxicos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgrotoxico(int id)
        {
            var usuarioId = GetUsuarioId();
            var agrotoxico = await _context.Agrotoxicos
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (agrotoxico == null)
            {
                return NotFound();
            }

            _context.Agrotoxicos.Remove(agrotoxico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgrotoxicoExists(int id)
        {
            return _context.Agrotoxicos.Any(e => e.Id == id);
        }
    }
}
