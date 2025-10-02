using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HankoSpa.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HankoSpa.Data
{
    // Definición de AppDbContext
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<CustomRol> CustomRoles { get; set; }
        public DbSet<RolPermission> RolPermissions { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Servicio> Servicios { get; set; } // <-- Agregado para mapear la entidad Servicio

        // Agrega aquí otros DbSet según tus modelos

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clave primaria compuesta para RolPermission
            modelBuilder.Entity<RolPermission>()
                .HasKey(rp => new { rp.CustomRolId, rp.PermissionId });
        }
    }

    public static class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // 1. Crear permisos base
            var permisos = new[]
            {
                new Permission { NombrePermiso = "Servicios_Create", Module = "Servicios" },
                new Permission { NombrePermiso = "Servicios_Read", Module = "Servicios" },
                new Permission { NombrePermiso = "Servicios_Update", Module = "Servicios" },
                new Permission { NombrePermiso = "Servicios_Delete", Module = "Servicios" },
                new Permission { NombrePermiso = "Citas_Create", Module = "Citas" },
                new Permission { NombrePermiso = "Citas_Read", Module = "Citas" },
                new Permission { NombrePermiso = "Citas_Update", Module = "Citas" },
                new Permission { NombrePermiso = "Citas_Delete", Module = "Citas" }
            };

            foreach (var permiso in permisos)
            {
                if (!context.Permissions.Any(p => p.NombrePermiso == permiso.NombrePermiso))
                    context.Permissions.Add(permiso);
            }
            await context.SaveChangesAsync();

            // 2. Crear roles base
            var roles = new[]
            {
                new CustomRol { NombreRol = "SuperUser" },
                new CustomRol { NombreRol = "Empleado" },
                new CustomRol { NombreRol = "Cliente" }
            };

            foreach (var rol in roles)
            {
                if (!context.CustomRoles.Any(r => r.NombreRol == rol.NombreRol))
                    context.CustomRoles.Add(rol);
            }
            await context.SaveChangesAsync();

            // 3. Asignar permisos a roles
            var allPerms = context.Permissions.ToList();
            var superUser = context.CustomRoles.First(r => r.NombreRol == "SuperUser");
            var empleado = context.CustomRoles.First(r => r.NombreRol == "Empleado");
            var cliente = context.CustomRoles.First(r => r.NombreRol == "Cliente");

            // SuperUser: todos los permisos
            foreach (var perm in allPerms)
            {
                if (!context.RolPermissions.Any(rp => rp.CustomRolId == superUser.CustomRolId && rp.PermissionId == perm.PermisoId))
                    context.RolPermissions.Add(new RolPermission { CustomRolId = superUser.CustomRolId, PermissionId = perm.PermisoId });
            }

            // Empleado: CRUD de Servicios y Citas
            var empleadoPerms = allPerms.Where(p => p.Module == "Servicios" || p.Module == "Citas");
            foreach (var perm in empleadoPerms)
            {
                if (!context.RolPermissions.Any(rp => rp.CustomRolId == empleado.CustomRolId && rp.PermissionId == perm.PermisoId))
                    context.RolPermissions.Add(new RolPermission { CustomRolId = empleado.CustomRolId, PermissionId = perm.PermisoId });
            }

            // Cliente: CRUD de Citas
            var clientePerms = allPerms.Where(p => p.Module == "Citas");
            foreach (var perm in clientePerms)
            {
                if (!context.RolPermissions.Any(rp => rp.CustomRolId == cliente.CustomRolId && rp.PermissionId == perm.PermisoId))
                    context.RolPermissions.Add(new RolPermission { CustomRolId = cliente.CustomRolId, PermissionId = perm.PermisoId });
            }

            await context.SaveChangesAsync();
        }
    }
}
