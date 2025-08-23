namespace API_TCC.Model
{
    public interface IEstoqueService
    {
        Task<bool> VerificarEstoqueSementeAsync(int sementeId, float quantidade, int usuarioId);
        Task<bool> VerificarEstoqueAgrotoxicoAsync(int agrotoxicoId, float quantidade, int usuarioId);
        Task<bool> VerificarEstoqueInsumoAsync(int insumoId, float quantidade, int usuarioId);
        
        Task<bool> BaixarEstoqueSementeAsync(int sementeId, float quantidade, int usuarioId);
        Task<bool> BaixarEstoqueAgrotoxicoAsync(int agrotoxicoId, float quantidade, int usuarioId);
        Task<bool> BaixarEstoqueInsumoAsync(int insumoId, float quantidade, int usuarioId);
        
        Task<bool> RetornarEstoqueSementeAsync(int sementeId, float quantidade, int usuarioId);
        Task<bool> RetornarEstoqueAgrotoxicoAsync(int agrotoxicoId, float quantidade, int usuarioId);
        Task<bool> RetornarEstoqueInsumoAsync(int insumoId, float quantidade, int usuarioId);
        
        Task<float> ObterEstoqueDisponivelSementeAsync(int sementeId, int usuarioId);
        Task<float> ObterEstoqueDisponivelAgrotoxicoAsync(int agrotoxicoId, int usuarioId);
        Task<float> ObterEstoqueDisponivelInsumoAsync(int insumoId, int usuarioId);
    }
}



