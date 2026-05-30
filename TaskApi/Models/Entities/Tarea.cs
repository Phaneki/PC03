using System.ComponentModel.DataAnnotations;
using TaskApi.Models.Enums;
using TaskApi.Models.Validations;

namespace TaskApi.Models.Entities
{
    public class Tarea
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public EstadoTarea Estado { get; set; }

        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        public PrioridadTarea Prioridad { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        [FechaVencimiento]
        public DateTime FechaVencimiento { get; set; }
    }
}
