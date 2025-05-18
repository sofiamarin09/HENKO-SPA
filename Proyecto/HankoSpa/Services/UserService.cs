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

namespace HankoSpa.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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

            // Mapea las propiedades editables desde DTO a entidad (sin tocar CustomRol directamente)
            _mapper.Map(userDto, user);

            // Actualizar el usuario
            var rowsAffected = await _userRepository.UpdateUserAsync(user);

            // Asignar el rol si cambió o está definido
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

    }
}