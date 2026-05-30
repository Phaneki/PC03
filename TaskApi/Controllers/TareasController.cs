using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Models.Entities;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TareasController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: api/tareas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas(
            [FromQuery] string? estado, 
            [FromQuery] string? prioridad, 
            [FromQuery] DateTime? fechaInicio, 
            [FromQuery] DateTime? fechaFin)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin.");
            }

            var query = _context.Tareas.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                if (Enum.TryParse<EstadoTarea>(estado, true, out var estadoEnum))
                {
                    query = query.Where(t => t.Estado == estadoEnum);
                }
                else
                {
                    return BadRequest($"El estado '{estado}' no es válido.");
                }
            }

            if (!string.IsNullOrEmpty(prioridad))
            {
                if (Enum.TryParse<PrioridadTarea>(prioridad, true, out var prioridadEnum))
                {
                    query = query.Where(t => t.Prioridad == prioridadEnum);
                }
                else
                {
                    return BadRequest($"La prioridad '{prioridad}' no es válida.");
                }
            }

            if (fechaInicio.HasValue)
            {
                query = query.Where(t => t.FechaVencimiento.Date >= fechaInicio.Value.Date);
            }
            if (fechaFin.HasValue)
            {
                query = query.Where(t => t.FechaVencimiento.Date <= fechaFin.Value.Date);
            }

            return await query.ToListAsync();
        }

        // GET: api/tareas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);

            if (tarea == null)
            {
                return NotFound();
            }

            return tarea;
        }

        // POST: api/tareas
        [HttpPost]
        public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
        }

        // PUT: api/tareas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(int id, Tarea tarea)
        {
            if (id != tarea.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID de la tarea.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/tareas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return NotFound();
            }

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.Id == id);
        }
    }
}
