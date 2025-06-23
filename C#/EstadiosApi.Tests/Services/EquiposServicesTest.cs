using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using EstadiosApi.Data;
using EstadiosApi.Services;
using EstadiosApi.Models;
using Moq;
using Microsoft.EntityFrameworkCore.ChangeTracking;




namespace EstadiosApi.Tests.Services
{
    public class EquiposServiceTests
    {
        private readonly EstadiosContext _context;
        private readonly EquiposService _equiposService;
        private readonly Mock<EstadiosContext> _mockContext;
        private readonly EquiposService _service;

        public EquiposServiceTests()
        {
            var options = new DbContextOptionsBuilder<EstadiosContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new EstadiosContext(options);
            _equiposService = new EquiposService(_context);
            _mockContext = new Mock<EstadiosContext>(new DbContextOptionsBuilder<EstadiosContext>().Options);
            _service = new EquiposService(_mockContext.Object);
        }

        [Fact]
        public async Task GetEquiposAsync_WhenNoEquipos_ReturnsEmptyList()
        {
            // Act
            var result = await _equiposService.GetEquiposAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task CreateEquipoAsync_AddsEquipoAndReturnsIt()
        {
            // Arrange
            var equipo = new Equipo { Nombre = "Equipo1", AñoFundacion = 1900, Colores = new List<string> { "Rojo", "Blanco" }, Pais = "Pais1", Ciudad = "Ciudad1" };

            // Act
            var created = await _equiposService.CreateEquipoAsync(equipo);

            // Assert
            Assert.Equal(equipo, created);
            Assert.Single(_context.Equipos);
            Assert.Equal("Equipo1", _context.Equipos.First().Nombre);
        }

        [Fact]
        public async Task GetEquipoAsync_WithExistingId_ReturnsEquipo()
        {
            // Arrange
            var equipo = new Equipo { Nombre = "Equipo2", AñoFundacion = 1950, Colores = new List<string> { "Azul" }, Pais = "Pais2", Ciudad = "Ciudad2" };
            _context.Equipos.Add(equipo);
            _context.SaveChanges();

            // Act
            var fetched = await _equiposService.GetEquipoAsync(equipo.Id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(equipo.Id, fetched.Id);
        }

        [Fact]
        public async Task GetEquipoAsync_WithNonexistentId_ReturnsNull()
        {
            // Act
            var fetched = await _equiposService.GetEquipoAsync(999);

            // Assert
            Assert.Null(fetched);
        }

        [Fact]
        public async Task UpdateEquipoAsync_WithMatchingId_UpdatesAndReturnsTrue()
        {
            // Arrange
            var equipo = new Equipo { Nombre = "Equipo3", AñoFundacion = 2000, Colores = new List<string> { "Verde" }, Pais = "Pais3", Ciudad = "Ciudad3" };
            _context.Equipos.Add(equipo);
            _context.SaveChanges();
            equipo.Nombre = "Equipo3Updated";

            // Act
            var result = await _equiposService.UpdateEquipoAsync(equipo.Id, equipo);

            // Assert
            Assert.True(result);
            var updated = await _equiposService.GetEquipoAsync(equipo.Id);
            Assert.Equal("Equipo3Updated", updated.Nombre);
        }

        [Fact]
        public async Task UpdateEquipoAsync_WithMismatchedId_ReturnsFalse()
        {
            // Arrange
            var equipo = new Equipo { Id = 1, Nombre = "EquipoX", AñoFundacion = 2000, Colores = new List<string> { "Negro" }, Pais = "PaisX", Ciudad = "CiudadX" };

            // Act
            var result = await _equiposService.UpdateEquipoAsync(2, equipo);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteEquipoAsync_WithExistingId_RemovesAndReturnsTrue()
        {
            // Arrange
            var equipo = new Equipo { Nombre = "Equipo4", AñoFundacion = 1980, Colores = new List<string> { "Amarillo" }, Pais = "Pais4", Ciudad = "Ciudad4" };
            _context.Equipos.Add(equipo);
            _context.SaveChanges();

            // Act
            var result = await _equiposService.DeleteEquipoAsync(equipo.Id);

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Equipos);
        }

        [Fact]
        public async Task DeleteEquipoAsync_WithNonexistentId_ReturnsFalse()
        {
            // Act
            var result = await _equiposService.DeleteEquipoAsync(999);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task GetEquiposAsync_WhenEquiposExist_ReturnsListOfEquipos()
        {
            // Arrange
            var equipo1 = new Equipo { Nombre = "E1", AñoFundacion = 1901, Colores = new List<string> { "A" }, Pais = "P1", Ciudad = "C1" };
            var equipo2 = new Equipo { Nombre = "E2", AñoFundacion = 1902, Colores = new List<string> { "B" }, Pais = "P2", Ciudad = "C2" };
            _context.Equipos.Add(equipo1);
            _context.Equipos.Add(equipo2);
            _context.SaveChanges();

            // Act
            var result = await _equiposService.GetEquiposAsync();

            // Assert
            Assert.NotEmpty(result);
            var list = result.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, e => e.Nombre == "E1");
            Assert.Contains(list, e => e.Nombre == "E2");
        }

        private EquiposService CreateServiceWithDb(out EstadiosContext context)
        {
            var options = new DbContextOptionsBuilder<EstadiosContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new EstadiosContext(options);
            return new EquiposService(context);
        }

        [Fact]
        public async Task UpdateEquipoAsync_WithMatchingIds_ReturnsTrue()
        {
            // Arrange
            var service = CreateServiceWithDb(out var context);

            var equipo = new Equipo { Id = 1, Nombre = "Equipo Original" ,AñoFundacion = 2000, Colores = new List<string> { "Negro" }, Pais = "PaisX", Ciudad = "CiudadX" };
            context.Equipos.Add(equipo);
            await context.SaveChangesAsync();

            equipo.Nombre = "Equipo Actualizado";

            // Act
            var result = await service.UpdateEquipoAsync(1, equipo);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateEquipoAsync_WithNonMatchingIds_ReturnsFalse()
        {
            var service = CreateServiceWithDb(out var context);

            var equipo = new Equipo { Id = 1, Nombre = "Equipo Test" , AñoFundacion = 2000, Colores = new List<string> { "Negro" }, Pais = "PaisX", Ciudad = "CiudadX" };

            var result = await service.UpdateEquipoAsync(2, equipo);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateEquipoAsync_DbUpdateConcurrencyException_EquipoNoExiste_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EstadiosContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new EstadiosContext(options);

            var service = new EquiposService(context);

            var equipo = new Equipo { Id = 1, Nombre = "Equipo Test", AñoFundacion = 2000, Colores = new List<string> { "Negro" }, Pais = "PaisX", Ciudad = "CiudadX"  };
            context.Equipos.Add(equipo);
            await context.SaveChangesAsync();

            // Eliminar el equipo para simular que no existe en el momento de SaveChangesAsync
            context.Equipos.Remove(equipo);
            await context.SaveChangesAsync();

            // Act
            var result = await service.UpdateEquipoAsync(1, equipo);

            // Assert
            Assert.False(result);
        }


        // Este test requiere control de concurrencia habilitado con [ConcurrencyCheck] o [Timestamp] en la entidad
        // Actualmente EF Core InMemory no lanza DbUpdateConcurrencyException sin ello.
        /*[Fact]
        public async Task UpdateEquipoAsync_DbUpdateConcurrencyException_EquipoExiste_ThrowsException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EstadiosContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new EstadiosContext(options);
            var service = new EquiposService(context);

            var equipo = new Equipo { Id = 1, Nombre = "Equipo Test" , AñoFundacion = 2000, Colores = new List<string> { "Negro" }, Pais = "PaisX", Ciudad = "CiudadX" };
            context.Equipos.Add(equipo);
            await context.SaveChangesAsync();

            // Forzar una excepción de concurrencia
            context.Entry(equipo).State = EntityState.Detached;

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await service.UpdateEquipoAsync(1, equipo);
            });
        }*/
    }
}
