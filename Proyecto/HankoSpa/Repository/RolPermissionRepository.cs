using HankoSpa.Data;
using HankoSpa.Models;
using Microsoft.EntityFrameworkCore;

namespace HankoSpa.Repository
{
    public class RolPermissionRepository : IRolPermissionRepository
    {
        private readonly AppDbContext _context;
        public RolPermissionRepository(AppDbContext context) => _context = context;

        public async Task<List<RolPermission>> GetByRolIdAsync(int rolId) =>
            await _context.RolPermissions
                .Include(rp => rp.Permission)
                .Where(rp => rp.CustomRolId == rolId)
                .ToListAsync();

        public async Task AddAsync(RolPermission rolPermission)
        {
            _context.RolPermissions.Add(rolPermission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int rolId, int permissionId)
        {
            var entity = await _context.RolPermissions
                .FirstOrDefaultAsync(rp => rp.CustomRolId == rolId && rp.PermissionId == permissionId);
            if (entity != null)
            {
                _context.RolPermissions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
