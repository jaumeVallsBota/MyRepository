using EstadiosApi.Data;
using EstadiosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EstadiosApi.Services
{
    public class EquiposService : IEquiposService
    {
        private readonly EstadiosContext _context;

        public EquiposService(EstadiosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Equipo>> GetEquiposAsync()
        {
            return await _context.Equipos.ToListAsync();
        }

        public async Task<Equipo?> GetEquipoAsync(int id)
        {
            return await _context.Equipos.FindAsync(id);
        }

        public async Task<Equipo> CreateEquipoAsync(Equipo equipo)
        {
            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();
            return equipo;
        }

        public async Task<bool> UpdateEquipoAsync(int id, Equipo equipo)
        {
            if (id != equipo.Id)
                return false;

            _context.Entry(equipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Equipos.Any(e => e.Id == id))
                    return false;

                throw;
            }
        }

        public async Task<bool> DeleteEquipoAsync(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
                return false;

            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}