using HankoSpa.Data;
using HankoSpa.Models;
using Microsoft.EntityFrameworkCore;

namespace HankoSpa.Repository
{

    public class CustomRolRepository : ICustomRolRepository
    {
        private readonly AppDbContext _context;

        public CustomRolRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los roles personalizados
        public async Task<List<CustomRol>> GetAllAsync()
        {
            return await _context.CustomRoles.ToListAsync();
        }
    }
}