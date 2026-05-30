using Microsoft.ML.Data;

namespace TaskApi.Models.ML
{
    public class TareaMLData
    {
        [LoadColumn(0)]
        public string Titulo { get; set; } = string.Empty;

        [LoadColumn(1)]
        public string Descripcion { get; set; } = string.Empty;

        [LoadColumn(2)]
        public string Prioridad { get; set; } = string.Empty;
    }
}
