using Microsoft.AspNetCore.Identity;
using HankoSpa.DTOs;

namespace HankoSpa.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<IdentityResult> CreateUserAsync(UserDTO userDto, string password);
        Task<bool> UpdateUserAsync(UserDTO userDto);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> AssignRoleToUserAsync(Guid userId, int customRolId);
        Task<List<CustomRolDTO>> GetAllRolesAsync();
    }
}
