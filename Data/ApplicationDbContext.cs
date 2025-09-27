using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pc2_VargasRenatoSebastian.Models;

namespace pc2_VargasRenatoSebastian.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Inmueble>()
                .HasIndex(i => i.Codigo)
                .IsUnique();
            builder.Entity<Inmueble>()
                .HasMany(i => i.Visitas)
                .WithOne(v => v.Inmueble)
                .HasForeignKey(v => v.InmuebleId);
            builder.Entity<Inmueble>()
                .HasMany(i => i.Reservas)
                .WithOne(r => r.Inmueble)
                .HasForeignKey(r => r.InmuebleId);

                builder.Entity<Inmueble>().HasData(
                    new Inmueble { Id = 1, Codigo = "DEP001", Titulo = "Departamento céntrico", Imagen = "dep1.jpg", Tipo = TipoInmueble.Departamento, Ciudad = "Lima", Direccion = "Av. Principal 123", Dormitorios = 2, Banos = 2, MetrosCuadrados = 80, Precio = 150000, Activo = true },
                    new Inmueble { Id = 2, Codigo = "CASA002", Titulo = "Casa familiar", Imagen = "casa2.jpg", Tipo = TipoInmueble.Casa, Ciudad = "Arequipa", Direccion = "Calle Secundaria 45", Dormitorios = 3, Banos = 3, MetrosCuadrados = 120, Precio = 250000, Activo = true },
                    new Inmueble { Id = 3, Codigo = "OFI003", Titulo = "Oficina moderna", Imagen = "ofi3.jpg", Tipo = TipoInmueble.Oficina, Ciudad = "Lima", Direccion = "Av. Empresarial 789", Dormitorios = 0, Banos = 2, MetrosCuadrados = 60, Precio = 100000, Activo = true },
                    new Inmueble { Id = 4, Codigo = "LOC004", Titulo = "Local comercial", Imagen = "loc4.jpg", Tipo = TipoInmueble.Local, Ciudad = "Cusco", Direccion = "Jr. Comercio 10", Dormitorios = 0, Banos = 1, MetrosCuadrados = 40, Precio = 80000, Activo = true }
                );
        }
    }
}
