using EstadiosApi.Data;
using EstadiosApi.Models;
using EstadiosApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EstadiosApi.Tests.Services
{
    public class EstadiosServicesTests
    {
        private EstadiosContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<EstadiosContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new EstadiosContext(options);
        }

        DateTime dateTime = DateTime.Now;

        [Fact]
        public async Task GetEstadiosAsync_ReturnsAllEstadios()
        {
            var context = GetInMemoryContext();
            context.Estadios.Add(new Estadio { Id = 1, Nombre = "Estadio 1", Ciudad = "Ciudad1", Pais = "Pais1", FotoUrl = "FotoURL1", Visitado = true, Latitud = 2.111111, Longitud = 2.111111, Aforo= 45454, EquipoId = null, Equipo = null, FechaInauguracion= dateTime });
            context.Estadios.Add(new Estadio { Id = 2, Nombre = "Estadio 2", Ciudad = "Ciudad2", Pais = "Pais2", FotoUrl = "FotoURL2", Visitado = true, Latitud = 2.111111, Longitud = 2.111111, Aforo= 45454, EquipoId = null, Equipo = null, FechaInauguracion= dateTime});
            await context.SaveChangesAsync();

            var service = new EstadiosService(context);

            var result = await service.GetEstadiosAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetEstadioAsync_ReturnsEstadio_WhenExists()
        {
            var context = GetInMemoryContext();
            context.Estadios.Add(new Estadio { Id = 1, Nombre = "Estadio 1", Ciudad = "Ciudad1", Pais = "Pais1", FotoUrl = "FotoURL1", Visitado = true, Latitud = 2.111111, Longitud = 2.111111, Aforo= 45454, EquipoId = null, Equipo = null, FechaInauguracion= dateTime  });
            await context.SaveChangesAsync();

            var service = new EstadiosService(context);

            var result = await service.GetEstadioAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Estadio 1", result.Nombre);
        }

        [Fact]
        public async Task GetEstadioAsync_ReturnsNull_WhenNotExists()
        {
            var context = GetInMemoryContext();
            var service = new EstadiosService(context);

            var result = await service.GetEstadioAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateEstadioAsync_AddsEstadio()
        {
            var context = GetInMemoryContext();
            var service = new EstadiosService(context);
            var estadio = new Estadio { Nombre = "Nuevo Estadio", Ciudad = "Ciudad1", Pais = "Pais1", FotoUrl = "FotoURL1", Visitado = true, Latitud = 2.111111, Longitud = 2.111111, Aforo= 45454, EquipoId = null, Equipo = null, FechaInauguracion= dateTime};

            var result = await service.CreateEstadioAsync(estadio);

            Assert.Equal("Nuevo Estadio", result.Nombre);
            Assert.Single(context.Estadios);
        }

        [Fact]
        public async Task UpdateEstadioAsync_ReturnsFalse_WhenIdMismatch()
        {
            var context = GetInMemoryContext();
            var service = new EstadiosService(context);
            var estadio = new Estadio { Id = 1, Nombre = "Estadio 1", Ciudad = "Ciudad1", Pais = "Pais1", FotoUrl = "FotoURL1", Visitado = true, Latitud = 2.111111, Longitud = 2.111111, Aforo= 45454, EquipoId = null, Equipo = null, FechaInauguracion= dateTime };

            var result = await service.UpdateEstadioAsync(2, estadio);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateEstadioAsync_UpdatesEstadio_WhenOk()
        {
            var context = GetInMemoryContext();
            var estadio = new Estadio { Id = 1, Nombre = "Estadio 1", Ciudad = "Ciudad1", Pais = "Pais1", FotoUrl = "FotoURL1", Visitado = true, Latitud = 2.111111, Longitud = 2.111111, Aforo= 45454, EquipoId = null, Equipo = null, FechaInauguracion= dateTime };
            context.Estadios.Add(estadio);
            await context.SaveChangesAsync();

            var service = new EstadiosService(context);
            estadio.Nombre = "Nuevo";

            var result = await service.UpdateEstadioAsync(1, estadio);

            Assert.True(result);
            Assert.Equal("Nuevo", context.Estadios.Find(1).Nombre);
        }

        [Fact]
        public async Task UpdateEstadioAsync_ReturnsFalse_WhenEstadioNotExists()
        {
            var context = GetInMemoryContext();
            var estadio = new Estadio { Id = 1, Nombre = "Estadio 1", Ciudad = "Ciudad1", Pais = "Pais1", FotoUrl = "FotoURL1", Visitado = true, Latitud = 2.111111, Longitud = 2.111111, Aforo= 45454, EquipoId = null, Equipo = null, FechaInauguracion= dateTime };

            var service = new EstadiosService(context);

            // simular DbUpdateConcurrencyException → usando control de concurrencia real
            var result = await service.UpdateEstadioAsync(1, estadio);

            // no existe, no lanza excepción porque InMemory no gestiona concurrencia por defecto
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteEstadioAsync_ReturnsTrue_WhenExists()
        {
            var context = GetInMemoryContext();
            var estadio = new Estadio { Id = 1, Nombre = "Estadio 1", Ciudad = "Ciudad1", Pais = "Pais1", FotoUrl = "FotoURL1", Visitado = true, Latitud = 2.111111, Longitud = 2.111111, Aforo= 45454, EquipoId = null, Equipo = null, FechaInauguracion= dateTime };
            context.Estadios.Add(estadio);
            await context.SaveChangesAsync();

            var service = new EstadiosService(context);

            var result = await service.DeleteEstadioAsync(1);

            Assert.True(result);
            Assert.Empty(context.Estadios);
        }

        [Fact]
        public async Task DeleteEstadioAsync_ReturnsFalse_WhenNotExists()
        {
            var context = GetInMemoryContext();
            var service = new EstadiosService(context);

            var result = await service.DeleteEstadioAsync(99);

            Assert.False(result);
        }

        [Fact]
        public void EstadioExists_ReturnsTrue_WhenExists()
        {
            var context = GetInMemoryContext();
            context.Estadios.Add(new Estadio { Id = 1, Nombre = "Estadio 1", Ciudad = "Ciudad1", Pais = "Pais1", FotoUrl = "FotoURL1", Visitado = true, Latitud = 2.111111, Longitud = 2.111111, Aforo= 45454, EquipoId = null, Equipo = null, FechaInauguracion= dateTime});
            context.SaveChanges();

            var service = new EstadiosService(context);

            var result = service.EstadioExists(1);

            Assert.True(result);
        }

        [Fact]
        public void EstadioExists_ReturnsFalse_WhenNotExists()
        {
            var context = GetInMemoryContext();
            var service = new EstadiosService(context);

            var result = service.EstadioExists(99);

            Assert.False(result);
        }
    }
}