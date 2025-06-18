using EstadiosApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace EstadiosApi.Data
{
    public class EstadiosContext : DbContext
    {
        public EstadiosContext(DbContextOptions<EstadiosContext> options)
            : base(options)
        {
        }
        public DbSet<Estadio> Estadios { get; set; }
        public DbSet<Equipo> Equipos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null));

            modelBuilder.Entity<Equipo>()
                .Property(e => e.Colores)
                .HasConversion(converter);
            modelBuilder.Entity<Estadio>()
                .Property(e => e.FechaInauguracion)
                .HasColumnType("timestamp without time zone");
        }

    }
}