using HankoSpa.Models;

namespace HankoSpa.Repository
{

    public interface ICustomRolRepository
    {
        // Obtener todos los roles personalizados
        Task<List<CustomRol>> GetAllAsync();
    }

}