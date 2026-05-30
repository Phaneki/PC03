using Microsoft.AspNetCore.Mvc;
using TaskApi.Models.DTOs;
using TaskApi.Services;

namespace TaskApi.Controllers
{
    [Route("api/tareas-externas")]
    [ApiController]
    public class TareasExternasController : ControllerBase
    {
        private readonly ITareasExternasService _tareasExternasService;

        public TareasExternasController(ITareasExternasService tareasExternasService)
        {
            _tareasExternasService = tareasExternasService;
        }

        // GET: api/tareas-externas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaExternaDto>>> GetTareasExternas()
        {
            try
            {
                var tareas = await _tareasExternasService.ObtenerTareasExternasAsync();
                
                if (tareas == null)
                {
                    return StatusCode(503, "El servicio externo de tareas no está disponible en este momento.");
                }

                return Ok(tareas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // GET: api/tareas-externas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaExternaDto>> GetTareaExterna(int id)
        {
            try
            {
                var tarea = await _tareasExternasService.ObtenerTareaExternaPorIdAsync(id);

                if (tarea == null)
                {
                    return NotFound($"No se encontró la tarea externa con el ID {id}.");
                }

                return Ok(tarea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }
    }
}
