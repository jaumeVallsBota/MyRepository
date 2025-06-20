using EstadiosApi.Models;

namespace EstadiosApi.Services
{
    public interface IEstadiosService
    {
        Task<IEnumerable<Estadio>> GetEstadiosAsync();
        Task<Estadio?> GetEstadioAsync(int id);
        Task<Estadio> CreateEstadioAsync(Estadio estadio);
        Task<bool> UpdateEstadioAsync(int id, Estadio estadio);
        Task<bool> DeleteEstadioAsync(int id);
        bool EstadioExists(int id);
    }
}