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
    public class EquiposController : ControllerBase
    {
        private readonly IEquiposService _equiposService;

        public EquiposController(IEquiposService equipoService)
        {
            _equiposService = equipoService;
        }

        // GET: api/Equipos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipos()
        {
            try
            {
                var result = await _equiposService.GetEquiposAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Equipos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> GetEquipo(int id)
        {
            var equipo = await _equiposService.GetEquipoAsync(id);

            if (equipo == null)
            {
                return NotFound();
            }

            return equipo;
        }

        // POST: api/Equipos
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Equipo>> PostEquipo(Equipo equipo)
        {

            var nuevoEquipo = await _equiposService.CreateEquipoAsync(equipo);

            return CreatedAtAction(nameof(GetEquipo), new { id = nuevoEquipo.Id }, nuevoEquipo);
        }

        // PUT: api/Equipos/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipo(int id, Equipo equipo)
        {

            var equipoactualizado = await _equiposService.UpdateEquipoAsync(id, equipo);
            if (!equipoactualizado)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Equipos/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipo(int id)
        {
            var eliminado = await _equiposService.DeleteEquipoAsync(id);

             if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}
