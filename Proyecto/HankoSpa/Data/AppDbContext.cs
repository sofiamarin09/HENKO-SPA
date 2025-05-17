using HankoSpa.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HankoSpa.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cita> Citas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Relacion entre Cita y Servicio
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Servicio) // Cita tiene un Servicio
                .WithMany(s => s.Citas) // Servicio tiene muchas Citas
                .HasForeignKey(c => c.ServicioId); // Clave foranea

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.User) // Cita tiene un User
                .WithMany(u => u.Citas) // User tiene muchas Citas
                .HasForeignKey(c => c.UsuarioID); // Clave for�nea

            DisableCascadingDelete(modelBuilder);
        }

        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}