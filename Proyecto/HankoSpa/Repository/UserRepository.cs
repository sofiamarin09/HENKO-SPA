using HankoSpa.Data;
using HankoSpa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HankoSpa.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(AppDbContext context,
                            UserManager<User> userManager,
                            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                                .Include(u => u.CustomRol)
                                .Include(u => u.Citas)
                                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                                .Include(u => u.CustomRol)
                                .Include(u => u.Citas)
                                .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public Task<IQueryable<User>> GetUsersQueryableAsync()
        {
            var query = _context.Users
                                .Include(u => u.CustomRol)
                                .Include(u => u.Citas)
                                .AsQueryable();
            return Task.FromResult(query);
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }
        public async Task<bool> AssignCustomRoleAsync(User user, int customRolId)
        {
            var customRole = await _context.CustomRoles.FindAsync(customRolId);
            if (customRole == null) return false;

            user.CustomRolId = customRolId;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CustomRoleExistsAsync(string roleName)
        {
            return await _context.CustomRoles.AnyAsync(r => r.NombreRol == roleName);
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

    }
}
