using EstadiosApi.Data;
using EstadiosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EstadiosApi.Services
{
    public class EstadiosService : IEstadiosService
    {
        private readonly EstadiosContext _context;

        public EstadiosService(EstadiosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estadio>> GetEstadiosAsync()
        {
            return await _context.Estadios.Include(e => e.Equipo).ToListAsync();
        }

        public async Task<Estadio?> GetEstadioAsync(int id)
        {
            return await _context.Estadios.Include(e => e.Equipo)
                                          .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Estadio> CreateEstadioAsync(Estadio estadio)
        {
            _context.Estadios.Add(estadio);
            await _context.SaveChangesAsync();
            return estadio;
        }

        public async Task<bool> UpdateEstadioAsync(int id, Estadio estadio)
        {
            if (id != estadio.Id)
                return false;

            _context.Entry(estadio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadioExists(id))
                    return false;

                throw;
            }
        }

        public async Task<bool> DeleteEstadioAsync(int id)
        {
            var estadio = await _context.Estadios.FindAsync(id);
            if (estadio == null)
                return false;

            _context.Estadios.Remove(estadio);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool EstadioExists(int id)
        {
            return _context.Estadios.Any(e => e.Id == id);
        }

    }









}