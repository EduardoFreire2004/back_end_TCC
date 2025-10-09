using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class PlantioDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int lavouraID { get; set; }
        public string? nomeLavoura { get; set; }
        public int sementeID { get; set; }
        public string? nomeSemente { get; set; }
        public string descricao { get; set; } = string.Empty;
        public DateTime dataHora { get; set; }
        public float areaPlantada { get; set; }
        public float qtde { get; set; }
    }

    public class PlantioCreateDTO
    {
        public int lavouraID { get; set; }
        public int sementeID { get; set; }
        public string descricao { get; set; } = string.Empty;
        public DateTime dataHora { get; set; }
        public float areaPlantada { get; set; }
        public float qtde { get; set; }
    }

    public class PlantioUpdateDTO
    {
        public int Id { get; set; }
        public int lavouraID { get; set; }
        public int sementeID { get; set; }
        public string descricao { get; set; } = string.Empty;
        public DateTime dataHora { get; set; }
        public float areaPlantada { get; set; }
        public float qtde { get; set; }
    }
}

