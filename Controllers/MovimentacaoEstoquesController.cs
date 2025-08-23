using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using API_TCC.DTOs;
using API_TCC.Enums;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    public class MovimentacaoEstoquesController : BaseController
    {
        private readonly Contexto _context;

        public MovimentacaoEstoquesController(Contexto context)
        {
            _context = context;
        }

        // GET: api/MovimentacaoEstoques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoque>>> GetMovimentacoesEstoque()
        {
            var usuarioId = GetUsuarioId();
            return await _context.MovimentacoesEstoque
                .Where(m => m.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // GET: api/MovimentacaoEstoques/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimentacaoEstoque>> GetMovimentacaoEstoque(int id)
        {
            var usuarioId = GetUsuarioId();
            var movimentacaoEstoque = await _context.MovimentacoesEstoque
                .Where(m => m.Id == id && m.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (movimentacaoEstoque == null)
            {
                return NotFound();
            }

            return movimentacaoEstoque;
        }

        // PUT: api/MovimentacaoEstoques/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimentacaoEstoque(int id, MovimentacaoEstoque movimentacaoEstoque)
        {
            var usuarioId = GetUsuarioId();
            
            if (id != movimentacaoEstoque.Id)
            {
                return BadRequest();
            }

            // Verificar se a movimentação pertence ao usuário
            var movimentacaoExistente = await _context.MovimentacoesEstoque
                .Where(m => m.Id == id && m.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (movimentacaoExistente == null)
            {
                return NotFound();
            }

            // Garantir que o UsuarioId não seja alterado
            movimentacaoEstoque.UsuarioId = usuarioId;

            _context.Entry(movimentacaoEstoque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimentacaoEstoqueExists(id, usuarioId))
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

        // POST: api/MovimentacaoEstoques
        [HttpPost]
        public async Task<ActionResult<MovimentacaoEstoque>> PostMovimentacaoEstoque(MovimentacaoEstoqueCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Garantir que apenas um tipo de item foi enviado (não nulo e diferente de zero)
            bool agrotoxicoInformado = dto.agrotoxicoID.HasValue && dto.agrotoxicoID.Value != 0;
            bool sementeInformada = dto.sementeID.HasValue && dto.sementeID.Value != 0;
            bool insumoInformado = dto.insumoID.HasValue && dto.insumoID.Value != 0;

            int tiposInformados = 0;
            if (agrotoxicoInformado) tiposInformados++;
            if (sementeInformada) tiposInformados++;
            if (insumoInformado) tiposInformados++;

            if (tiposInformados != 1)
                return BadRequest("Informe apenas um tipo de item (Agrotóxico, Semente ou Insumo) por movimentação.");

            bool estoqueAtualizado = false;

            // 1️⃣ Agrotóxico
            if (agrotoxicoInformado)
            {
                var agrotoxico = await _context.Agrotoxicos.FindAsync(dto.agrotoxicoID.Value);
                if (agrotoxico == null)
                    return NotFound("Agrotóxico não encontrado.");

                if (dto.movimentacao == TipoMovimentacao.Saida && agrotoxico.qtde < dto.qtde)
                    return BadRequest("Estoque insuficiente para saída.");

                agrotoxico.qtde += dto.movimentacao == TipoMovimentacao.Entrada ? dto.qtde : -dto.qtde;
                estoqueAtualizado = true;
            }

            // 2️⃣ Semente
            if (sementeInformada)
            {
                var semente = await _context.Sementes.FindAsync(dto.sementeID.Value);
                if (semente == null)
                    return NotFound("Semente não encontrada.");

                if (dto.movimentacao == TipoMovimentacao.Saida && semente.qtde < dto.qtde)
                    return BadRequest("Estoque insuficiente para saída.");

                semente.qtde += dto.movimentacao == TipoMovimentacao.Entrada ? dto.qtde : -dto.qtde;
                estoqueAtualizado = true;
            }

            // 3️⃣ Insumo
            if (insumoInformado)
            {
                var insumo = await _context.Insumos.FindAsync(dto.insumoID.Value);
                if (insumo == null)
                    return NotFound("Insumo não encontrado.");

                if (dto.movimentacao == TipoMovimentacao.Saida && insumo.qtde < dto.qtde)
                    return BadRequest("Estoque insuficiente para saída.");

                insumo.qtde += dto.movimentacao == TipoMovimentacao.Entrada ? dto.qtde : -dto.qtde;
                estoqueAtualizado = true;
            }

            if (!estoqueAtualizado)
                return BadRequest("Erro ao atualizar estoque.");

            var movimentacao = new MovimentacaoEstoque
            {
                lavouraID = dto.lavouraID,
                movimentacao = dto.movimentacao,
                agrotoxicoID = agrotoxicoInformado ? dto.agrotoxicoID : null,
                sementeID = sementeInformada ? dto.sementeID : null,
                insumoID = insumoInformado ? dto.insumoID : null,
                qtde = dto.qtde,
                dataHora = dto.dataHora,
                descricao = dto.descricao,
                UsuarioId = GetUsuarioId()
            };

            _context.MovimentacoesEstoque.Add(movimentacao);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Erro ao salvar movimentação: {ex.Message}");
            }

            return CreatedAtAction("GetMovimentacaoEstoque", new { id = movimentacao.Id }, movimentacao);
        }


        // DELETE: api/MovimentacaoEstoques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimentacaoEstoque(int id)
        {
            var usuarioId = GetUsuarioId();
            var movimentacaoEstoque = await _context.MovimentacoesEstoque
                .Where(m => m.Id == id && m.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (movimentacaoEstoque == null)
            {
                return NotFound();
            }

            _context.MovimentacoesEstoque.Remove(movimentacaoEstoque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovimentacaoEstoqueExists(int id, int usuarioId)
        {
            return _context.MovimentacoesEstoque.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}
