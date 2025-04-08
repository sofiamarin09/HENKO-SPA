using HankoSpa.DTOs;
using HankoSpa.Nucleo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HankoSpa.Services.Interfaces
{
    public interface ICitasService
    {
        Task<Response<List<CitasDTO>>> GetAllAsync();
        Task<Response<CitasDTO>> GetOneAsync(int id);
        Task<Response<CitasDTO>> CreateAsync(CitasDTO dto);
        Task<Response<CitasDTO>> EditAsync(CitasDTO dto);
        Task<Response<object>> DeleteAsync(int id);
    }
}
