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

            // Validações básicas
            if (dto.qtde <= 0)
                return BadRequest("A quantidade deve ser maior que zero.");

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

            // Restringir para apenas entradas vindas desta tela
            if (dto.movimentacao != TipoMovimentacao.Entrada)
                return BadRequest("Apenas movimentações do tipo Entrada são permitidas nesta tela. Saídas são geradas automaticamente por Aplicação, Plantio e Aplicação de Insumo.");

            // Usar transação para garantir consistência
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                bool estoqueAtualizado = false;

                // 1️⃣ Agrotóxico
                if (agrotoxicoInformado)
                {
                    var agrotoxico = await _context.Agrotoxicos
                        .Where(a => a.Id == dto.agrotoxicoID.Value && a.UsuarioId == GetUsuarioId())
                        .FirstOrDefaultAsync();
                    
                    if (agrotoxico == null)
                        return NotFound("Agrotóxico não encontrado.");

                    // Apenas entrada
                    agrotoxico.qtde += dto.qtde;
                    
                    // Garantir que o estoque não fique negativo
                    if (agrotoxico.qtde < 0)
                        agrotoxico.qtde = 0;
                    
                    estoqueAtualizado = true;
                }

                // 2️⃣ Semente
                if (sementeInformada)
                {
                    var semente = await _context.Sementes
                        .Where(s => s.Id == dto.sementeID.Value && s.UsuarioId == GetUsuarioId())
                        .FirstOrDefaultAsync();
                    
                    if (semente == null)
                        return NotFound("Semente não encontrada.");

                    // Apenas entrada
                    semente.qtde += dto.qtde;
                    
                    // Garantir que o estoque não fique negativo
                    if (semente.qtde < 0)
                        semente.qtde = 0;
                    
                    estoqueAtualizado = true;
                }

                // 3️⃣ Insumo
                if (insumoInformado)
                {
                    var insumo = await _context.Insumos
                        .Where(i => i.Id == dto.insumoID.Value && i.UsuarioId == GetUsuarioId())
                        .FirstOrDefaultAsync();
                    
                    if (insumo == null)
                        return NotFound("Insumo não encontrado.");

                    // Apenas entrada
                    insumo.qtde += dto.qtde;
                    
                    // Garantir que o estoque não fique negativo
                    if (insumo.qtde < 0)
                        insumo.qtde = 0;
                    
                    estoqueAtualizado = true;
                }

                if (!estoqueAtualizado)
                    return BadRequest("Erro ao atualizar estoque.");

                var movimentacao = new MovimentacaoEstoque
                {
                    lavouraID = dto.lavouraID,
                    movimentacao = TipoMovimentacao.Entrada,
                    agrotoxicoID = agrotoxicoInformado ? dto.agrotoxicoID : null,
                    sementeID = sementeInformada ? dto.sementeID : null,
                    insumoID = insumoInformado ? dto.insumoID : null,
                    qtde = dto.qtde,
                    dataHora = dto.dataHora,
                    descricao = dto.descricao,
                    UsuarioId = GetUsuarioId()
                };

                _context.MovimentacoesEstoque.Add(movimentacao);

                // Salvar todas as alterações
                await _context.SaveChangesAsync();
                
                // Commit da transação
                await transaction.CommitAsync();

                return CreatedAtAction("GetMovimentacaoEstoque", new { id = movimentacao.Id }, movimentacao);
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest($"Erro ao processar movimentação: {ex.Message}");
            }
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
