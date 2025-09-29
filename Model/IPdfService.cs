using API_TCC.DTOs;

namespace API_TCC.Model
{
    public interface IPdfService
    {
        byte[] GerarRelatorioCustos(RelatorioCustosDto relatorio);
        byte[] GerarRelatorioInsumos(RelatorioInsumosDto relatorio);
        byte[] GerarRelatorioAgrotoxicos(RelatorioAgrotoxicosDto relatorio);
        byte[] GerarRelatorioSementes(RelatorioSementesDto relatorio);
        byte[] GerarRelatorioPlantios(RelatorioPlantiosDto relatorio);
        byte[] GerarRelatorioAplicacoes(RelatorioAplicacoesDto relatorio);
        byte[] GerarRelatorioColheitas(RelatorioColheitasDto relatorio);
        byte[] GerarRelatorioEstoque(RelatorioEstoqueDto relatorio);
        byte[] GerarRelatorioGeral(RelatorioGeralDto relatorio);
    }
}















