using System.ComponentModel.DataAnnotations;

namespace API_TCC.DTOs
{
    public class LavouraDto
    {
        [Required]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

        [Required]
        [Range(0, float.MaxValue)]
        public float area { get; set; }

        [Required]
        public float latitude { get; set; }

        [Required]
        public float longitude { get; set; }
    }

    public class LavouraResponseDto
    {
        public int Id { get; set; }
        public string nome { get; set; } = string.Empty;
        public float area { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
    }
}
