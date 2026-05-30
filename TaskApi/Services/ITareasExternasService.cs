using TaskApi.Models.DTOs;

namespace TaskApi.Services
{
    public interface ITareasExternasService
    {
        Task<IEnumerable<TareaExternaDto>?> ObtenerTareasExternasAsync();
        Task<TareaExternaDto?> ObtenerTareaExternaPorIdAsync(int id);
    }
}
