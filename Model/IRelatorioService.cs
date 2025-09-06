using API_TCC.DTOs;

namespace API_TCC.Model
{
    public interface IRelatorioService
    {
        // Relatórios específicos por tipo de produto
        Task<RelatorioAgrotoxicosDto> GerarRelatorioAgrotoxicosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
        Task<RelatorioInsumosDto> GerarRelatorioInsumosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
        Task<RelatorioSementesDto> GerarRelatorioSementesAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
        
        // Relatórios existentes
        Task<RelatorioGeralDto> GerarRelatorioGeralAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
        Task<RelatorioPlantiosDto> GerarRelatorioPlantiosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
        Task<RelatorioAplicacoesDto> GerarRelatorioAplicacoesAgrotoxicosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
        Task<RelatorioAplicacoesDto> GerarRelatorioAplicacoesInsumosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
        Task<RelatorioColheitasDto> GerarRelatorioColheitasAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
        Task<RelatorioCustosDto> GerarRelatorioCustosAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
        Task<RelatorioEstoqueDto> GerarRelatorioEstoqueAsync(int lavouraId, DateTime dataInicio, DateTime dataFim);
    }
}
