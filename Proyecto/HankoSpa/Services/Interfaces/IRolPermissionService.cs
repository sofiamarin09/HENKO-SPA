using HankoSpa.DTOs;
using HankoSpa.Nucleo;

namespace HankoSpa.Services.Interfaces
{
    public interface IRolPermissionService
    {
        Task<Response<List<RolPermissionDTO>>> GetPermissionsByRolIdAsync(int rolId);
        Task<Response<bool>> AssignPermissionAsync(RolPermissionDTO dto);
        Task<Response<bool>> RemovePermissionAsync(int rolId, int permissionId);
    }
}
