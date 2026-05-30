using Microsoft.ML.Data;

namespace TaskApi.Models.ML
{
    public class TareaMLPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PrioridadRecomendada { get; set; } = string.Empty;
    }
}
