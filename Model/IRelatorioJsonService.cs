using API_TCC.DTOs;

namespace API_TCC.Model
{
    public interface IRelatorioJsonService
    {
        Task<RelatorioFornecedoresResponseDto> GerarRelatorioFornecedoresAsync(int usuarioId, int lavouraId);
        Task<RelatorioAplicacaoResponseDto> GerarRelatorioAplicacaoAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioAplicacaoInsumoResponseDto> GerarRelatorioAplicacaoInsumoAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioSementeResponseDto> GerarRelatorioSementeAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioInsumoResponseDto> GerarRelatorioInsumoAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioAgrotoxicoResponseDto> GerarRelatorioAgrotoxicoAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioColheitaResponseDto> GerarRelatorioColheitaAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioMovimentacaoEstoqueResponseDto> GerarRelatorioMovimentacaoEstoqueAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<RelatorioPlantioResponseDto> GerarRelatorioPlantioAsync(int usuarioId, int lavouraId, DateTime? dataInicio = null, DateTime? dataFim = null);
    }
}





