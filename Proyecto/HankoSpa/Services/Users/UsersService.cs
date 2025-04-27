using HankoSpa.Models;
using HankoSpa.Repository.Users;
using Microsoft.AspNetCore.Identity;

namespace HankoSpa.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _usersRepository;

        public UsersService(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password) => await _usersRepository.AddUserAsync(user, password);

        public async Task AddUserToRoleAsync(User user, string roleName) => await _usersRepository.AddUserToRoleAsync(user, roleName);

        public async Task CheckRoleAsync(string roleName) => await _usersRepository.CheckRoleAsync(roleName);

        public async Task<User> GetUserAsync(string email) => await _usersRepository.GetUserAsync(email);

        public async Task<bool> IsUserInRoleAsync(User user, string roleName) => await _usersRepository.IsUserInRoleAsync(user, roleName);

    }
}
