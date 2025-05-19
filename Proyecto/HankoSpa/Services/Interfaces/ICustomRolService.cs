using HankoSpa.DTOs;
using HankoSpa.Nucleo;

namespace HankoSpa.Services.Interfaces
{
    public interface ICustomRolService
    {
        Task<Response<List<CustomRolDTO>>> GetAllAsync();
        Task<Response<CustomRolDTO>> GetByIdAsync(int id);
        Task<Response<CustomRolDTO>> CreateAsync(CustomRolDTO dto);
        Task<Response<CustomRolDTO>> UpdateAsync(CustomRolDTO dto);
        Task<Response<bool>> DeleteAsync(int id);
    }
}
