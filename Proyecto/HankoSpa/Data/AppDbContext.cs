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
        public DbSet<CitasServicios> CitasServicios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CitasServicios>()
                .HasOne(cs => cs.Cita)
                .WithMany(c => c.CitasServicios)
                .HasForeignKey(cs => cs.CitasID);

            modelBuilder.Entity<CitasServicios>()
                .HasOne(cs => cs.Servicio)
                .WithMany(s => s.CitasServicios)
                .HasForeignKey(cs => cs.ServicioID);

            modelBuilder.Entity<User>().HasNoKey();
        }
    }
}