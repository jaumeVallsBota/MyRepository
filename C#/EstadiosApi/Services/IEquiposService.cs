using EstadiosApi.Models;

namespace EstadiosApi.Services
{
    public interface IEquiposService
    {
        Task<IEnumerable<Equipo>> GetEquiposAsync();
        Task<Equipo?> GetEquipoAsync(int id);
        Task<Equipo> CreateEquipoAsync(Equipo equipo);
        Task<bool> UpdateEquipoAsync(int id, Equipo equipo);
        Task<bool> DeleteEquipoAsync(int id);
    }
}