using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HankoSpa.DTOs;
using HankoSpa.Models;
using HankoSpa.Repository;
using HankoSpa.Services;
using Microsoft.AspNetCore.Identity;
using HankoSpa.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using HankoSpa.Nucleo;
using HankoSpa.Data;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using HankoSpa.Core;
using NuGet.Protocol.Core.Types;


namespace HankoSpa.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICustomRolService _customRolSerivce;
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IMapper mapper, ICustomRolService customRolSerivce, AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _customRolSerivce = customRolSerivce;
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var usersQuery = await _userRepository.GetUsersQueryableAsync();
            var usersList = await usersQuery.ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(usersList);
        }

        public async Task<IdentityResult> CreateUserAsync(UserDTO userDto, string password)
        {
            userDto.Id = Guid.NewGuid().ToString();
            var user = _mapper.Map<User>(userDto);
            var result = await _userRepository.AddUserAsync(user, password);
            if (!result.Succeeded) return result;

            // Asignar rol al usuario (si hay rol válido)
            if (userDto.CustomRolId > 0)
            {
                await _userRepository.AssignCustomRoleAsync(user, userDto.CustomRolId);
            }

            return result;
        }

        public async Task<bool> UpdateUserAsync(UserDTO userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userDto.Id!));
            if (user == null) return false;

            _mapper.Map(userDto, user);

            System.Diagnostics.Debug.WriteLine($"Usuario después del mapeo: {user.FirstName}, {user.LastName}, {user.Email}");

            var rowsAffected = await _userRepository.UpdateUserAsync(user);

            if (user.CustomRolId != userDto.CustomRolId && userDto.CustomRolId > 0)
            {
                await _userRepository.AssignCustomRoleAsync(user, userDto.CustomRolId);
            }

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            var result = await _userRepository.DeleteUserAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, int customRolId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            return await _userRepository.AssignCustomRoleAsync(user, customRolId);
        }

        public async Task<List<CustomRolDTO>> GetAllRolesAsync()
        {
            var response = await _customRolSerivce.GetAllAsync();
            return response.Result ?? new List<CustomRolDTO>();
        }
        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users.Include(c => c.CustomRol).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userRepository.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userRepository.ConfirmEmailAsync(user, token);
        }
        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            var es = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
            return es;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public bool CurrentUserIsAuthenticated()
        {
            ClaimsUser? user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity != null && user.Identity.IsAuthenticated;
        }

        public async Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module)
        {
            ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;
            if (claimsUser is null)
            {
                return false;
            }
            string? userName = claimsUser.Identity!.Name;

            User? user = await GetUserAsync(userName);

            if (user is null)
            {
                return true;
            }

            if (user.CustomRol.NombreRol == Env.SUPERADMINROLENAME)
                return true;

            return await _context.Permissions.Include(r => r.RolPermissions).AnyAsync(p => (p.Module == module && p.NombrePermiso == permission)
            && p.RolPermissions.Any(rp => rp.CustomRolId == user.CustomRolId));

        }

        public async Task<Response<List<UserDTO>>> GetAllUsersComboAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                var dtoList = _mapper.Map<List<UserDTO>>(users);
                return new Response<List<UserDTO>>(true, "Servicios obtenidos correctamente.", dtoList);
            }
            catch (Exception ex)
            {
                return HandleException<List<UserDTO>>(ex, "Error al obtener los servicios.");
            }
        }
        // Manejo centralizado de excepciones
        private Response<T> HandleException<T>(Exception ex, string message)
        {
            return new Response<T>(false, message)
            {
                Errors = new List<string> { ex.Message }
            };
        }
    }
}