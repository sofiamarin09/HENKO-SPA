using HankoSpa.Data;
using HankoSpa.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HankoSpa.Repository
{
    public class CustomRolRepository : ICustomRolRepository
    {
        private readonly AppDbContext _context;
        public CustomRolRepository(AppDbContext context) => _context = context;

        public async Task<List<CustomRol>> GetAllAsync() => await _context.CustomRoles.ToListAsync();

        public async Task<CustomRol?> GetByIdAsync(int id) => await _context.CustomRoles.FindAsync(id);

        public async Task AddAsync(CustomRol rol)
        {
            _context.CustomRoles.Add(rol);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CustomRol rol)
        {
            _context.CustomRoles.Update(rol);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.CustomRoles.FindAsync(id);
            if (entity != null)
            {
                _context.CustomRoles.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // Inicializa roles y permisos si no existen
        public async Task InitializeRolesAndPermissionsAsync()
        {
            await AppDbContextSeed.SeedAsync(_context);
        }

        // Asigna permisos automáticamente al crear roles base
        public async Task AssignBasePermissionsAsync(int customRolId, string nombreRol)
        {
            var allPerms = await _context.Permissions.ToListAsync();
            if (nombreRol == "SuperUser")
            {
                foreach (var perm in allPerms)
                {
                    if (!_context.RolPermissions.Any(rp => rp.CustomRolId == customRolId && rp.PermissionId == perm.PermisoId))
                        _context.RolPermissions.Add(new RolPermission { CustomRolId = customRolId, PermissionId = perm.PermisoId });
                }
            }
            else if (nombreRol == "Empleado")
            {
                foreach (var perm in allPerms.Where(p => p.Module == "Servicios" || p.Module == "Citas"))
                {
                    if (!_context.RolPermissions.Any(rp => rp.CustomRolId == customRolId && rp.PermissionId == perm.PermisoId))
                        _context.RolPermissions.Add(new RolPermission { CustomRolId = customRolId, PermissionId = perm.PermisoId });
                }
            }
            else if (nombreRol == "Cliente")
            {
                foreach (var perm in allPerms.Where(p => p.Module == "Citas"))
                {
                    if (!_context.RolPermissions.Any(rp => rp.CustomRolId == customRolId && rp.PermissionId == perm.PermisoId))
                        _context.RolPermissions.Add(new RolPermission { CustomRolId = customRolId, PermissionId = perm.PermisoId });
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
