using Microsoft.AspNetCore.Mvc;
using TaskApi.Models.DTOs;
using TaskApi.Services;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MlController : ControllerBase
    {
        private readonly ITareaMlService _tareaMlService;

        public MlController(ITareaMlService tareaMlService)
        {
            _tareaMlService = tareaMlService;
        }

        [HttpPost("recomendar-prioridad")]
        public ActionResult<object> RecomendarPrioridad([FromBody] RecomendacionRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Titulo))
            {
                return BadRequest("El título es obligatorio para generar una recomendación.");
            }

            var prioridad = _tareaMlService.PredecirPrioridad(request.Titulo, request.Descripcion);

            return Ok(new
            {
                prioridadRecomendada = prioridad
            });
        }
    }
}
