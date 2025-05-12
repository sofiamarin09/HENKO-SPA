using HankoSpa.Models;
using Microsoft.AspNetCore.Identity;

namespace HankoSpa.Services.Users
{
    public interface IUsersService
    {
        Task<User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

    }
}
