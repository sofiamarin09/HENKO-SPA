using HankoSpa.Data;
using HankoSpa.Models;
using Microsoft.EntityFrameworkCore;

namespace HankoSpa.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;
        public PermissionRepository(AppDbContext context) => _context = context;

        public async Task<List<Permission>> GetAllAsync() => await _context.Permissions.ToListAsync();

        public async Task<Permission?> GetByIdAsync(int id) => await _context.Permissions.FindAsync(id);

        public async Task AddAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Permissions.FindAsync(id);
            if (entity != null)
            {
                _context.Permissions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

