using System.Collections.Generic;
using System.Threading.Tasks;
using HankoSpa.DTOs;
using HankoSpa.Models;

namespace HankoSpa.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<CustomRolDTO>> GetRolesAsync();
        Task<IEnumerable<User>> GetUsuariosAsync();
    }
}

