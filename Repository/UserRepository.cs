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
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(AppDbContext context,
                            UserManager<User> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
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

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _context.Users
             .FirstOrDefaultAsync(x => x.Email == email);
            return user!;
        }
    }
}
