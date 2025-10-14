using API_TCC.DTOs;

namespace API_TCC.Model
{
    public interface ICustoService
    {
        /// <summary>
        /// Calcula custos totais de uma lavoura baseado em movimentações, aplicações e aplicações de insumos
        /// </summary>
        Task<CustoCalculadoDto> CalcularCustosLavouraAsync(int lavouraId, int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
        
        /// <summary>
        /// Calcula custos de uma aplicação específica
        /// </summary>
        Task<CustoAplicacaoDto> CalcularCustoAplicacaoAsync(int aplicacaoId, int usuarioId);
        
        /// <summary>
        /// Calcula custos de uma aplicação de insumo específica
        /// </summary>
        Task<CustoAplicacaoInsumoDto> CalcularCustoAplicacaoInsumoAsync(int aplicacaoInsumoId, int usuarioId);
        
        /// <summary>
        /// Obtém resumo de custos por período
        /// </summary>
        Task<ResumoCustosDto> ObterResumoCustosAsync(int lavouraId, int usuarioId, DateTime dataInicio, DateTime dataFim);
        
        /// <summary>
        /// Obtém histórico de custos de uma lavoura
        /// </summary>
        Task<List<HistoricoCustoDto>> ObterHistoricoCustosAsync(int lavouraId, int usuarioId, DateTime? dataInicio = null, DateTime? dataFim = null);
        
        /// <summary>
        /// Atualiza custos de uma lavoura (recalcula tudo)
        /// </summary>
        Task<bool> AtualizarCustosLavouraAsync(int lavouraId, int usuarioId);
    }
}












