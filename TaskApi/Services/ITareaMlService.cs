using TaskApi.Models.ML;

namespace TaskApi.Services
{
    public interface ITareaMlService
    {
        string PredecirPrioridad(string titulo, string descripcion);
    }
}
