using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HankoSpa.DTOs;
using HankoSpa.Models;
using HankoSpa.Data;

namespace HankoSpa.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly AppDbContext _context;

        public CombosHelper(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomRolDTO>> GetRolesAsync()
        {
            // Mapea los roles a DTOs. Ajusta según tu lógica de mapeo.
            return await _context.CustomRoles
                .Select(r => new CustomRolDTO
                {
                    CustomRolId = r.CustomRolId,
                    NombreRol = r.NombreRol
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsuariosAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}

