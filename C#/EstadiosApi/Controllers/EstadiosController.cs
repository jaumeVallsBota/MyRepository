using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstadiosApi.Data;
using EstadiosApi.Models;
using Microsoft.AspNetCore.Authorization;
using EstadiosApi.Services;

namespace EstadiosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadiosController : ControllerBase
    {
        private readonly IEstadiosService _estadiosService;

        public EstadiosController(IEstadiosService estadiosService)
        {
            _estadiosService = estadiosService;
        }

        

        // GET: api/Estadios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estadio>>> GetEstadios()
        {

            try
            {
                var result = await _estadiosService.GetEstadiosAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // GET: api/Estadios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estadio>> GetEstadio(int id)
        {
            var estadio = await _estadiosService.GetEstadioAsync(id);

            if (estadio == null)
                return NotFound();

            return Ok(estadio);
        
        }

        // POST: api/Estadios
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Estadio>> PostEstadio(Estadio estadio)
        {
            var nuevoEstadio = await _estadiosService.CreateEstadioAsync(estadio);
            return CreatedAtAction(nameof(GetEstadio), new { id = nuevoEstadio.Id }, nuevoEstadio);
        }

        // PUT: api/Estadios/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadio(int id, Estadio estadio)
        {
            var actualizado = await _estadiosService.UpdateEstadioAsync(id, estadio);
            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Estadios/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadio(int id)
        {
            var eliminado = await _estadiosService.DeleteEstadioAsync(id);
            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}