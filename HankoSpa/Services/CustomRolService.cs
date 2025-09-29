using AutoMapper;
using HankoSpa.DTOs;
using HankoSpa.Models;
using HankoSpa.Nucleo;
using HankoSpa.Repository;
using HankoSpa.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HankoSpa.Services
{
    public class CustomRolService : ICustomRolService
    {
        private readonly ICustomRolRepository _customRolRepository;
        private readonly IMapper _mapper;

        public CustomRolService(ICustomRolRepository customRolRepository, IMapper mapper)
        {
            _customRolRepository = customRolRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<CustomRolDTO>>> GetAllAsync()
        {
            var customRoles = await _customRolRepository.GetAllAsync();
            var dtoList = _mapper.Map<List<CustomRolDTO>>(customRoles);
            return new Response<List<CustomRolDTO>>(true, "Roles obtenidos correctamente.", dtoList);
        }

        public async Task<Response<CustomRolDTO>> GetByIdAsync(int id)
        {
            var entity = await _customRolRepository.GetByIdAsync(id);
            if (entity == null)
                return new Response<CustomRolDTO>(false, "Rol no encontrado");
            return new Response<CustomRolDTO>(true, "OK", _mapper.Map<CustomRolDTO>(entity));
        }

        public async Task<Response<CustomRolDTO>> CreateAsync(CustomRolDTO dto)
        {
            var entity = _mapper.Map<CustomRol>(dto);
            await _customRolRepository.AddAsync(entity);
            // Asignar permisos base automáticamente
            await _customRolRepository.AssignBasePermissionsAsync(entity.CustomRolId, entity.NombreRol);
            return new Response<CustomRolDTO>(true, "Rol creado correctamente", _mapper.Map<CustomRolDTO>(entity));
        }

        public async Task<Response<CustomRolDTO>> UpdateAsync(CustomRolDTO dto)
        {
            var entity = await _customRolRepository.GetByIdAsync(dto.CustomRolId);
            if (entity == null)
                return new Response<CustomRolDTO>(false, "Rol no encontrado");
            _mapper.Map(dto, entity);
            await _customRolRepository.UpdateAsync(entity);
            return new Response<CustomRolDTO>(true, "Rol actualizado correctamente", _mapper.Map<CustomRolDTO>(entity));
        }

        public async Task<Response<bool>> DeleteAsync(int id)
        {
            await _customRolRepository.DeleteAsync(id);
            return new Response<bool>(true, "Rol eliminado correctamente", true);
        }

        // Métodos agregados
        public async Task InitializeRolesAndPermissionsAsync()
        {
            await _customRolRepository.InitializeRolesAndPermissionsAsync();
        }

        public async Task AssignBasePermissionsAsync(int customRolId, string nombreRol)
        {
            await _customRolRepository.AssignBasePermissionsAsync(customRolId, nombreRol);
        }
    }
}
