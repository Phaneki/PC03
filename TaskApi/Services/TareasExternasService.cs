using System.Text.Json;
using TaskApi.Models.DTOs;

namespace TaskApi.Services
{
    public class TareasExternasService : ITareasExternasService
    {
        private readonly HttpClient _httpClient;

        public TareasExternasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TareaExternaDto>?> ObtenerTareasExternasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var todos = JsonSerializer.Deserialize<List<JsonPlaceholderTodoDto>>(jsonString, options);

                if (todos == null) return new List<TareaExternaDto>();

                return todos.Select(t => new TareaExternaDto
                {
                    ExternalId = t.Id,
                    Titulo = t.Title,
                    Completado = t.Completed
                });
            }
            catch (HttpRequestException)
            {
                throw new Exception("Error de red al intentar conectar con la API externa.");
            }
        }

        public async Task<TareaExternaDto?> ObtenerTareaExternaPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/todos/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null; // Devuelve nulo para lanzar 404 en el controlador
                    }
                    throw new Exception($"La API externa respondió con estado de error: {response.StatusCode}");
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var todo = JsonSerializer.Deserialize<JsonPlaceholderTodoDto>(jsonString, options);

                if (todo == null) return null;

                return new TareaExternaDto
                {
                    ExternalId = todo.Id,
                    Titulo = todo.Title,
                    Completado = todo.Completed
                };
            }
            catch (HttpRequestException)
            {
                throw new Exception("Error de red al intentar conectar con la API externa.");
            }
        }
    }
}
