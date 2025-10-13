namespace API_TCC.DTOs
{
    namespace API_TCC.DTOs
    {
        public class InsumoDTO
        {
            public int Id { get; set; }
            public int CategoriaInsumoID { get; set; }
            public int FornecedorInsumoID { get; set; }
            public string Nome { get; set; }
            public string Unidade_Medida { get; set; }
            public DateTime Data_Cadastro { get; set; }
            public double Qtde { get; set; }
            public double Preco { get; set; }
        }
    }

}

