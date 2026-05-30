using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models.Validations
{
    public class FechaVencimientoAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime fechaVencimiento)
            {
                if (fechaVencimiento.Date < DateTime.UtcNow.Date)
                {
                    return new ValidationResult("La fecha de vencimiento no puede ser menor a la fecha actual.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
