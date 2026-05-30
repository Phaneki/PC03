using Microsoft.ML;
using TaskApi.Models.ML;

namespace TaskApi.Services
{
    public class TareaMlService : ITareaMlService
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<TareaMLData, TareaMLPrediction> _predictionEngine;

        public TareaMlService()
        {
            _mlContext = new MLContext();
            
            // 1. Cargar datos desde el CSV
            var dataPath = Path.Combine(Environment.CurrentDirectory, "ML", "priority-dataset.csv");
            IDataView dataView = _mlContext.Data.LoadFromTextFile<TareaMLData>(dataPath, hasHeader: true, separatorChar: ',');

            // 2. Crear Pipeline (Transformar texto a números y asignar entrenador de Clasificación Multiclase)
            var pipeline = _mlContext.Transforms.Text.FeaturizeText("TituloFeaturized", nameof(TareaMLData.Titulo))
                .Append(_mlContext.Transforms.Text.FeaturizeText("DescripcionFeaturized", nameof(TareaMLData.Descripcion)))
                .Append(_mlContext.Transforms.Concatenate("Features", "TituloFeaturized", "DescripcionFeaturized"))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(TareaMLData.Prioridad)))
                .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // 3. Entrenar Modelo
            var model = pipeline.Fit(dataView);

            // 4. Crear Motor de Predicción
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<TareaMLData, TareaMLPrediction>(model);
        }

        public string PredecirPrioridad(string titulo, string descripcion)
        {
            var input = new TareaMLData
            {
                Titulo = titulo,
                Descripcion = descripcion
            };

            var prediction = _predictionEngine.Predict(input);
            return prediction.PrioridadRecomendada;
        }
    }
}
