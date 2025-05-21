using HankoSpa.DTOs;
using HankoSpa.Nucleo;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HankoSpa.Services.Interfaces
{
    public interface ICustomRolService
    {
        Task<Response<List<CustomRolDTO>>> GetAllAsync();
        Task<Response<CustomRolDTO>> GetByIdAsync(int id);
        Task<Response<CustomRolDTO>> CreateAsync(CustomRolDTO dto);
        Task<Response<CustomRolDTO>> UpdateAsync(CustomRolDTO dto);
        Task<Response<bool>> DeleteAsync(int id);

        // Métodos agregados
        Task InitializeRolesAndPermissionsAsync();
        Task AssignBasePermissionsAsync(int customRolId, string nombreRol);
    }
}
