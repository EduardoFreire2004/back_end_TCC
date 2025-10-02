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
    public class AplicacoesController : BaseController
    {
        private readonly Contexto _context;
        private readonly IEstoqueService _estoqueService;

        public AplicacoesController(Contexto context, IEstoqueService estoqueService)
        {
            _context = context;
            _estoqueService = estoqueService;
        }

        // GET: api/Aplicacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AplicacaoResponseDto>>> GetAplicacoes()
        {
            var usuarioId = GetUsuarioId();
            var aplicacoes = await _context.Aplicacoes
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.lavoura)
                .Include(a => a.agrotoxico)
                .Select(a => new AplicacaoResponseDto
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    lavouraID = a.lavouraID,
                    agrotoxicoID = a.agrotoxicoID,
                    qtde = a.qtde,
                    dataHora = a.dataHora,
                    descricao = a.descricao,
                    LavouraNome = a.lavoura.nome,
                    AgrotoxicoNome = a.agrotoxico.nome,
                    AgrotoxicoUnidadeMedida = a.agrotoxico.unidade_Medida
                })
                .ToListAsync();

            return aplicacoes;
        }

        // GET: api/Aplicacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AplicacaoResponseDto>> GetAplicacao(int id)
        {
            var usuarioId = GetUsuarioId();
            var aplicacao = await _context.Aplicacoes
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .Include(a => a.lavoura)
                .Include(a => a.agrotoxico)
                .Select(a => new AplicacaoResponseDto
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    lavouraID = a.lavouraID,
                    agrotoxicoID = a.agrotoxicoID,
                    qtde = a.qtde,
                    dataHora = a.dataHora,
                    descricao = a.descricao,
                    LavouraNome = a.lavoura.nome,
                    AgrotoxicoNome = a.agrotoxico.nome,
                    AgrotoxicoUnidadeMedida = a.agrotoxico.unidade_Medida
                })
                .FirstOrDefaultAsync();

            if (aplicacao == null)
            {
                return NotFound();
            }

            return aplicacao;
        }

        // GET: api/Aplicacoes/porlavoura/5
        [HttpGet("porlavoura/{lavouraId}")]
        public async Task<ActionResult<IEnumerable<AplicacaoResponseDto>>> GetAplicacoesPorLavoura(int lavouraId)
        {
            var usuarioId = GetUsuarioId();
            var aplicacoes = await _context.Aplicacoes
                .Where(a => a.lavouraID == lavouraId && a.UsuarioId == usuarioId)
                .Include(a => a.lavoura)
                .Include(a => a.agrotoxico)
                .Select(a => new AplicacaoResponseDto
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    lavouraID = a.lavouraID,
                    agrotoxicoID = a.agrotoxicoID,
                    qtde = a.qtde,
                    dataHora = a.dataHora,
                    descricao = a.descricao,
                    LavouraNome = a.lavoura.nome,
                    AgrotoxicoNome = a.agrotoxico.nome,
                    AgrotoxicoUnidadeMedida = a.agrotoxico.unidade_Medida
                })
                .ToListAsync();

            return aplicacoes;
        }

        // PUT: api/Aplicacoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAplicacao(int id, AplicacaoUpdateDto dto)
        {
            var usuarioId = GetUsuarioId();
            
            if (id != dto.Id)
            {
                return BadRequest();
            }

            // Verificar se a aplicação pertence ao usuário
            var aplicacaoExistente = await _context.Aplicacoes
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (aplicacaoExistente == null)
            {
                return NotFound();
            }

            // Usar transação para garantir consistência
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Verificar se houve mudança na quantidade
                if (dto.qtde != aplicacaoExistente.qtde)
                {
                    // Se a nova quantidade for maior, verificar se há estoque suficiente
                    if (dto.qtde > aplicacaoExistente.qtde)
                    {
                        var quantidadeAdicional = dto.qtde - aplicacaoExistente.qtde;
                        var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelAgrotoxicoAsync(dto.agrotoxicoID, usuarioId);
                        
                        if (estoqueDisponivel < quantidadeAdicional)
                        {
                            return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Necessário: {quantidadeAdicional}" });
                        }
                        
                        // Baixar estoque adicional
                        var baixouEstoque = await _estoqueService.BaixarEstoqueAgrotoxicoAsync(dto.agrotoxicoID, quantidadeAdicional, usuarioId);
                        if (!baixouEstoque)
                        {
                            return BadRequest(new { message = "Erro ao baixar estoque adicional" });
                        }
                    }
                    // Se a nova quantidade for menor, retornar diferença ao estoque
                    else if (dto.qtde < aplicacaoExistente.qtde)
                    {
                        var quantidadeRetornar = aplicacaoExistente.qtde - dto.qtde;
                        var retornouEstoque = await _estoqueService.RetornarEstoqueAgrotoxicoAsync(dto.agrotoxicoID, quantidadeRetornar, usuarioId);
                        if (!retornouEstoque)
                        {
                            return BadRequest(new { message = "Erro ao retornar estoque" });
                        }
                    }
                }

                // Atualizar a aplicação
                aplicacaoExistente.lavouraID = dto.lavouraID;
                aplicacaoExistente.agrotoxicoID = dto.agrotoxicoID;
                aplicacaoExistente.qtde = dto.qtde;
                aplicacaoExistente.dataHora = dto.dataHora;
                aplicacaoExistente.descricao = dto.descricao;

                _context.Entry(aplicacaoExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                // Commit da transação
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao atualizar aplicação: {ex.Message}" });
            }

            return NoContent();
        }

        // POST: api/Aplicacoes
        [HttpPost]
        public async Task<ActionResult<AplicacaoResponseDto>> PostAplicacao(AplicacaoCreateDto dto)
        {
            var usuarioId = GetUsuarioId();
            
            // Verificar se há estoque suficiente
            var estoqueDisponivel = await _estoqueService.ObterEstoqueDisponivelAgrotoxicoAsync(dto.agrotoxicoID, usuarioId);
            if (estoqueDisponivel < dto.qtde)
            {
                return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoqueDisponivel}, Solicitado: {dto.qtde}" });
            }
            
            // Usar transação para garantir consistência
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Baixar estoque
                var baixouEstoque = await _estoqueService.BaixarEstoqueAgrotoxicoAsync(dto.agrotoxicoID, dto.qtde, usuarioId);
                if (!baixouEstoque)
                {
                    return BadRequest(new { message = "Erro ao baixar estoque" });
                }
                
                var aplicacao = new Aplicacao
                {
                    lavouraID = dto.lavouraID,
                    agrotoxicoID = dto.agrotoxicoID,
                    qtde = dto.qtde,
                    dataHora = dto.dataHora,
                    descricao = dto.descricao,
                    UsuarioId = usuarioId
                };
                
                _context.Aplicacoes.Add(aplicacao);
                await _context.SaveChangesAsync();

                // Registrar movimentação de saída no estoque
                var movimentacao = new MovimentacaoEstoque
                {
                    lavouraID = aplicacao.lavouraID,
                    movimentacao = TipoMovimentacao.Saida,
                    agrotoxicoID = aplicacao.agrotoxicoID,
                    sementeID = null,
                    insumoID = null,
                    qtde = aplicacao.qtde,
                    dataHora = aplicacao.dataHora,
                    descricao = string.IsNullOrWhiteSpace(aplicacao.descricao) ? "Saída por Aplicação de Agrotóxico" : $"Saída por Aplicação de Agrotóxico - {aplicacao.descricao}",
                    UsuarioId = usuarioId,
                    origemAplicacaoID = aplicacao.Id
                };
                _context.MovimentacoesEstoque.Add(movimentacao);
                await _context.SaveChangesAsync();
                
                // Commit da transação
                await transaction.CommitAsync();
                
                // Retornar a aplicação criada com dados de navegação
                var aplicacaoCriada = await _context.Aplicacoes
                    .Where(a => a.Id == aplicacao.Id)
                    .Include(a => a.lavoura)
                    .Include(a => a.agrotoxico)
                    .Select(a => new AplicacaoResponseDto
                    {
                        Id = a.Id,
                        UsuarioId = a.UsuarioId,
                        lavouraID = a.lavouraID,
                        agrotoxicoID = a.agrotoxicoID,
                        qtde = a.qtde,
                        dataHora = a.dataHora,
                        descricao = a.descricao,
                        LavouraNome = a.lavoura.nome,
                        AgrotoxicoNome = a.agrotoxico.nome,
                        AgrotoxicoUnidadeMedida = a.agrotoxico.unidade_Medida
                    })
                    .FirstOrDefaultAsync();

                return CreatedAtAction("GetAplicacao", new { id = aplicacao.Id }, aplicacaoCriada);
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao criar aplicação: {ex.Message}" });
            }
        }

        // DELETE: api/Aplicacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAplicacao(int id)
        {
            var usuarioId = GetUsuarioId();
            var aplicacao = await _context.Aplicacoes
                .Where(a => a.Id == id && a.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();
                
            if (aplicacao == null)
            {
                return NotFound();
            }

            // Usar transação para garantir consistência
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Retornar quantidade ao estoque
                var retornouEstoque = await _estoqueService.RetornarEstoqueAgrotoxicoAsync(aplicacao.agrotoxicoID, aplicacao.qtde, usuarioId);
                if (!retornouEstoque)
                {
                    return BadRequest(new { message = "Erro ao retornar estoque" });
                }

                _context.Aplicacoes.Remove(aplicacao);
                await _context.SaveChangesAsync();
                
                // Commit da transação
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest(new { message = $"Erro ao excluir aplicação: {ex.Message}" });
            }

            return NoContent();
        }

        private bool AplicacaoExists(int id, int usuarioId)
        {
            return _context.Aplicacoes.Any(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}
