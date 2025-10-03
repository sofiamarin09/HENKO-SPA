using HankoSpa.Models;
using Microsoft.EntityFrameworkCore;

namespace HankoSpa.Data.Seeder
{
    public class PermissionsSeeder
    {
        private readonly AppDbContext _context;

        public PermissionsSeeder(AppDbContext context)
        {
            _context = context;
        }
        public async Task SeedAsync()
        {
            List<Permission> permissions = [.. Section()];

            foreach (Permission permission in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.NombrePermiso == permission.NombrePermiso && p.Module == permission.Module);

                if (!exists)
                {
                    await _context.Permissions.AddAsync(permission);
                }
            }

            await _context.SaveChangesAsync();

        }

        private List<Permission> Section()
        {
            return new List<Permission>
            {
                new Permission {NombrePermiso = "showSection", Module = "Secciones" },
                new Permission {NombrePermiso = "createSection", Module = "Secciones" },
                new Permission {NombrePermiso = "updateSection", Module = "Secciones" },
                new Permission {NombrePermiso = "deleteSection", Module = "Secciones" }
            };
        }
    }
}
