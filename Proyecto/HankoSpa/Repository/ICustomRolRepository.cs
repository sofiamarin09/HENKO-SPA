using HankoSpa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HankoSpa.Repository
{
    public interface ICustomRolRepository
    {
        Task<List<CustomRol>> GetAllAsync();
        Task<CustomRol?> GetByIdAsync(int id);
        Task AddAsync(CustomRol rol);
        Task UpdateAsync(CustomRol rol);
        Task DeleteAsync(int id);

        // Métodos agregados
        Task InitializeRolesAndPermissionsAsync();
        Task AssignBasePermissionsAsync(int customRolId, string nombreRol);
    }
}
