using API_TCC.DTOs;

namespace API_TCC.Model
{
    public interface IRelatorioJsonService
    {
        Task<RelatorioFornecedoresResponseDto> GerarRelatorioFornecedoresAsync(int usuarioId);
        Task<RelatorioAplicacaoResponseDto> GerarRelatorioAplicacaoAsync(int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioAplicacaoInsumoResponseDto> GerarRelatorioAplicacaoInsumoAsync(int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioSementeResponseDto> GerarRelatorioSementeAsync(int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioInsumoResponseDto> GerarRelatorioInsumoAsync(int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioAgrotoxicoResponseDto> GerarRelatorioAgrotoxicoAsync(int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioColheitaResponseDto> GerarRelatorioColheitaAsync(int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioMovimentacaoEstoqueResponseDto> GerarRelatorioMovimentacaoEstoqueAsync(int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioPlantioResponseDto> GerarRelatorioPlantioAsync(int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
    }
}

