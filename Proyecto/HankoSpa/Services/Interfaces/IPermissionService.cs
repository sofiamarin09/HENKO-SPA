using HankoSpa.DTOs;
using HankoSpa.Models;
using HankoSpa.Nucleo;

namespace HankoSpa.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<Response<List<PermissionDTO>>> GetAllAsync();
        Task<Response<PermissionDTO>> GetByIdAsync(int id);
        Task<Response<PermissionDTO>> CreateAsync(PermissionDTO dto);
        Task<Response<PermissionDTO>> UpdateAsync(PermissionDTO dto);
        Task<Response<bool>> DeleteAsync(int id);
    }
}

