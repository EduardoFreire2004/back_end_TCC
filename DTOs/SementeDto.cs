using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    namespace API_TCC.DTOs
    {
        public class SementeDTO
        {
            public int Id { get; set; }
            public int FornecedorID { get; set; }
            public string Nome { get; set; }
            public string Tipo { get; set; }
            public string Marca { get; set; }
            public double Qtde { get; set; }
            public double Preco { get; set; }
            public DateTime Data_Cadastro { get; set; }
        }
    }

}

