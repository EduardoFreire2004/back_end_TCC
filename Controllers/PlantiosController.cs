using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;
using API_TCC.Enums;
using API_TCC.DTOs;

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
        public async Task<ActionResult<IEnumerable<PlantioDTO>>> GetPlantios()
        {
            var usuarioId = GetUsuarioId();

            var plantios = await _context.Plantios
                .Include(p => p.semente)
                .Include(p => p.lavoura)
                .Where(p => p.UsuarioId == usuarioId)
                .Select(p => new PlantioDTO
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    lavouraID = p.lavouraID,
                    nomeLavoura = p.lavoura != null ? p.lavoura.nome : string.Empty,
                    sementeID = p.sementeID,
                    nomeSemente = p.semente != null ? p.semente.nome : string.Empty,
                    descricao = p.descricao,
                    dataHora = p.dataHora,
                    areaPlantada = p.areaPlantada,
                    qtde = p.qtde
                })
                .ToListAsync();

            return Ok(plantios);
        }

        // GET: api/Plantios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlantioDTO>> GetPlantio(int id)
        {
            var usuarioId = GetUsuarioId();

            var plantio = await _context.Plantios
                .Include(p => p.semente)
                .Include(p => p.lavoura)
                .Where(p => p.Id == id && p.UsuarioId == usuarioId)
                .Select(p => new PlantioDTO
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    lavouraID = p.lavouraID,
                    nomeLavoura = p.lavoura != null ? p.lavoura.nome : string.Empty,
                    sementeID = p.sementeID,
                    nomeSemente = p.semente != null ? p.semente.nome : string.Empty,
                    descricao = p.descricao,
                    dataHora = p.dataHora,
                    areaPlantada = p.areaPlantada,
                    qtde = p.qtde
                })
                .FirstOrDefaultAsync();

            if (plantio == null)
                return NotFound();

            return Ok(plantio);
        }

        // GET: api/Plantios/porlavoura/5
        [HttpGet("porlavoura/{lavouraId}")]
        public async Task<ActionResult<IEnumerable<PlantioDTO>>> GetPlantioPorLavoura(int lavouraId)
        {
            var usuarioId = GetUsuarioId();

            var plantios = await _context.Plantios
                .Include(p => p.semente)
                .Include(p => p.lavoura)
                .Where(p => p.lavouraID == lavouraId && p.UsuarioId == usuarioId)
                .Select(p => new PlantioDTO
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    lavouraID = p.lavouraID,
                    nomeLavoura = p.lavoura != null ? p.lavoura.nome : string.Empty,
                    sementeID = p.sementeID,
                    nomeSemente = p.semente != null ? p.semente.nome : string.Empty,
                    descricao = p.descricao,
                    dataHora = p.dataHora,
                    areaPlantada = p.areaPlantada,
                    qtde = p.qtde
                })
                .ToListAsync();

            return Ok(plantios);
        }

        // GET: api/Plantios/porlavoura/{lavouraId}/buscar?nome={nome}
        [HttpGet("porlavoura/{lavouraId}/buscar")]
        public async Task<ActionResult<IEnumerable<PlantioDTO>>> GetByNome(string nome, int lavouraId)
        {
            try
            {
                var usuarioId = GetUsuarioId();

                var plantios = await _context.Plantios
                    .Include(p => p.semente)
                    .Include(p => p.lavoura)
                    .Where(p => p.lavouraID == lavouraId &&
                                p.UsuarioId == usuarioId &&
                                p.semente != null &&
                                EF.Functions.Like(p.semente.nome, $"%{nome}%"))
                    .Select(p => new PlantioDTO
                    {
                        Id = p.Id,
                        UsuarioId = p.UsuarioId,
                        lavouraID = p.lavouraID,
                        nomeLavoura = p.lavoura != null ? p.lavoura.nome : string.Empty,
                        sementeID = p.sementeID,
                        nomeSemente = p.semente != null ? p.semente.nome : string.Empty,
                        descricao = p.descricao,
                        dataHora = p.dataHora,
                        areaPlantada = p.areaPlantada,
                        qtde = p.qtde
                    })
                    .ToListAsync();

                return Ok(plantios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao buscar por {nome}: {ex.Message}" });
            }
        }

        // PUT: api/Plantios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlantio(int id, PlantioUpdateDTO dto)
        {
            var usuarioId = GetUsuarioId();

            if (id != dto.Id)
                return BadRequest();

            var plantioExistente = await _context.Plantios
                .Where(p => p.Id == id && p.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (plantioExistente == null)
                return NotFound();

            // Controle de estoque se houve mudança de quantidade
            if (dto.qtde != plantioExistente.qtde)
            {
                if (dto.qtde > plantioExistente.qtde)
                {
                    var adicional = dto.qtde - plantioExistente.qtde;
                    var estoque = await _estoqueService.ObterEstoqueDisponivelSementeAsync(dto.sementeID, usuarioId);

                    if (estoque < adicional)
                        return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoque}, Necessário: {adicional}" });

                    await _estoqueService.BaixarEstoqueSementeAsync(dto.sementeID, adicional, usuarioId);
                }
                else
                {
                    var retorno = plantioExistente.qtde - dto.qtde;
                    await _estoqueService.RetornarEstoqueSementeAsync(dto.sementeID, retorno, usuarioId);
                }
            }

            // Atualiza os campos necessários
            plantioExistente.lavouraID = dto.lavouraID;
            plantioExistente.sementeID = dto.sementeID;
            plantioExistente.descricao = dto.descricao;
            plantioExistente.dataHora = dto.dataHora;
            plantioExistente.areaPlantada = dto.areaPlantada;
            plantioExistente.qtde = dto.qtde;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Plantios
        [HttpPost]
        public async Task<ActionResult<PlantioDTO>> PostPlantio(PlantioCreateDTO dto)
        {
            var usuarioId = GetUsuarioId();

            if (!await _estoqueService.VerificarEstoqueSementeAsync(dto.sementeID, dto.qtde, usuarioId))
            {
                var estoque = await _estoqueService.ObterEstoqueDisponivelSementeAsync(dto.sementeID, usuarioId);
                return BadRequest(new { message = $"Estoque insuficiente. Disponível: {estoque}, Solicitado: {dto.qtde}" });
            }

            await _estoqueService.BaixarEstoqueSementeAsync(dto.sementeID, dto.qtde, usuarioId);

            var plantio = new Plantio
            {
                lavouraID = dto.lavouraID,
                sementeID = dto.sementeID,
                descricao = dto.descricao,
                dataHora = dto.dataHora,
                areaPlantada = dto.areaPlantada,
                qtde = dto.qtde,
                UsuarioId = usuarioId
            };

            _context.Plantios.Add(plantio);
            await _context.SaveChangesAsync();

            // Registrar movimentação após salvar (para ter o Id)
            var movimentacao = new MovimentacaoEstoque
            {
                lavouraID = plantio.lavouraID,
                movimentacao = TipoMovimentacao.Saida,
                sementeID = plantio.sementeID,
                qtde = plantio.qtde,
                dataHora = plantio.dataHora,
                descricao = string.IsNullOrWhiteSpace(dto.descricao)
                    ? "Saída por Plantio de Semente"
                    : $"Saída por Plantio de Semente - {dto.descricao}",
                UsuarioId = usuarioId,
                origemPlantioID = plantio.Id
            };

            _context.MovimentacoesEstoque.Add(movimentacao);
            await _context.SaveChangesAsync();

            // Retorno formatado em DTO
            var result = new PlantioDTO
            {
                Id = plantio.Id,
                UsuarioId = usuarioId,
                lavouraID = plantio.lavouraID,
                sementeID = plantio.sementeID,
                descricao = plantio.descricao,
                dataHora = plantio.dataHora,
                areaPlantada = plantio.areaPlantada,
                qtde = plantio.qtde
            };

            return CreatedAtAction(nameof(GetPlantio), new { id = plantio.Id }, result);
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
                return NotFound();

            await _estoqueService.RetornarEstoqueSementeAsync(plantio.sementeID, plantio.qtde, usuarioId);

            _context.Plantios.Remove(plantio);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Plantio excluído com sucesso" });
        }

        private bool PlantioExists(int id, int usuarioId)
        {
            return _context.Plantios.Any(p => p.Id == id && p.UsuarioId == usuarioId);
        }
    }
}
