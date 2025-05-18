using HankoSpa.DTOs;
using HankoSpa.Nucleo;

namespace HankoSpa.Services.Interfaces
{
    public interface ICustomRolService
    {
        Task<Response<List<CustomRolDTO>>> GetAllAsync();
    }
}