using System.ComponentModel.DataAnnotations;

namespace EstadiosApi.Models
{
    public class Equipo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del equipo es obligatorio")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        [RegularExpression(@"\S.*", ErrorMessage = "El nombre no puede ser solo espacios en blanco")]
        public string Nombre { get; set; }

        [Range(1800, 2100, ErrorMessage = "El año de fundación debe estar entre 1800 y 2100")]
        public int AñoFundacion { get; set; }

        [Required(ErrorMessage = "Debe indicar los colores")]
        public List<string> Colores { get; set; } = new List<string>();

        [Required(ErrorMessage = "El país es obligatorio")]
        [RegularExpression(@"\S.*", ErrorMessage = "El país no puede ser solo espacios en blanco")]
        public string Pais { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [RegularExpression(@"\S.*", ErrorMessage = "La ciudad no puede ser solo espacios en blanco")]
        public string Ciudad { get; set; }
    }
}