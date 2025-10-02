using Microsoft.AspNetCore.Identity;
using HankoSpa.DTOs;
using HankoSpa.Nucleo;
using HankoSpa.Models;

namespace HankoSpa.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<List<UserDTO>>> GetAllUsersComboAsync();
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<IdentityResult> CreateUserAsync(UserDTO userDto, string password);
        Task<bool> UpdateUserAsync(UserDTO userDto);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> AssignRoleToUserAsync(Guid userId, int customRolId);
        Task<List<CustomRolDTO>> GetAllRolesAsync();
        public Task<IdentityResult> AddUserAsync(User user, string password);
        public bool CurrentUserIsAuthenticated();
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        public Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        public Task<string> GenerateEmailConfirmationTokenAsync(User user);
        public Task<User> GetUserAsync(string email);
        public Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();

    }
}
