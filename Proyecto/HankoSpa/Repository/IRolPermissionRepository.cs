using HankoSpa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HankoSpa.Repository
{
    public interface IRolPermissionRepository
    {
        Task<List<RolPermission>> GetByRolIdAsync(int rolId);
        Task AddAsync(RolPermission rolPermission);
        Task DeleteAsync(int rolId, int permissionId);
    }
}

