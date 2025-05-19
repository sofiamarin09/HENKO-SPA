using HankoSpa.Data;
using HankoSpa.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
