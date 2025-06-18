using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstadiosApi.Data;
using EstadiosApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace EstadiosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadiosController : ControllerBase
    {
        private readonly EstadiosContext _context;

        public EstadiosController(EstadiosContext context)
        {
            _context = context;
        }

        // GET: api/Estadios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estadio>>> GetEstadios()
        {
            return await _context.Estadios.Include(e => e.Equipo).ToListAsync();
        }

        // GET: api/Estadios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estadio>> GetEstadio(int id)
        {
            var estadio = await _context.Estadios.Include(e => e.Equipo).FirstOrDefaultAsync(e => e.Id == id);;

            if (estadio == null)
            {
                return NotFound();
            }

            return estadio;
        }

        // POST: api/Estadios
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Estadio>> PostEstadio([FromBody]Estadio estadio)
        {
            _context.Estadios.Add(estadio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstadio), new { id = estadio.Id }, estadio);
        }

        // PUT: api/Estadios/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadio(int id, Estadio estadio)
        {
            if (id != estadio.Id)
            {
                return BadRequest();
            }

            _context.Entry(estadio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadioExists(id))
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

        // DELETE: api/Estadios/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadio(int id)
        {
            var estadio = await _context.Estadios.FindAsync(id);
            if (estadio == null)
            {
                return NotFound();
            }

            _context.Estadios.Remove(estadio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadioExists(int id)
        {
            return _context.Estadios.Any(e => e.Id == id);
        }
    }
}