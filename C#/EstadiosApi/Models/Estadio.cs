using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace EstadiosApi.Models
{
    public class Estadio
    {
         public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del estadio es obligatorio")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        [RegularExpression(@"\S.*", ErrorMessage = "El nombre no puede ser solo espacios en blanco")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El nombre de la ciudad es obligatorio")]
        [RegularExpression(@"\S.*", ErrorMessage = "La ciudad no puede ser solo espacios en blanco")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "El nombre del país es obligatorio")]
        [RegularExpression(@"\S.*", ErrorMessage = "El país no puede ser solo espacios en blanco")]
        public string Pais { get; set; }

        public bool Visitado { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El aforo debe ser un número positivo")]
        public int Aforo { get; set; }

        public DateTime FechaInauguracion { get; set; }

        // Relación con Equipo
        public int? EquipoId { get; set; }
        public Equipo? Equipo { get; set; }
    }
}