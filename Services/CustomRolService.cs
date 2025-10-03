using AutoMapper;
using HankoSpa.DTOs;
using HankoSpa.Models;
using HankoSpa.Nucleo;
using HankoSpa.Repository;
using HankoSpa.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using HankoSpa.Services; // Agregado para usar SimpleCacheManager

namespace HankoSpa.Services
{
    public class CustomRolService : ICustomRolService
    {
        private readonly ICustomRolRepository _customRolRepository;
        private readonly IMapper _mapper;
        private const string CacheKey = "Roles";

        public CustomRolService(ICustomRolRepository customRolRepository, IMapper mapper)
        {
            _customRolRepository = customRolRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<CustomRolDTO>>> GetAllAsync()
        {
            // Intentar obtener de caché
            var cached = SimpleCacheManager.Instance.Get<List<CustomRolDTO>>(CacheKey);
            if (cached != null)
            {
                return new Response<List<CustomRolDTO>>(true, "Roles obtenidos correctamente. (desde caché)", cached);
            }

            var customRoles = await _customRolRepository.GetAllAsync();
            var dtoList = _mapper.Map<List<CustomRolDTO>>(customRoles);

            // Guardar en caché
            SimpleCacheManager.Instance.Set(CacheKey, dtoList);

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

            // Limpiar caché porque los datos han cambiado
            SimpleCacheManager.Instance.Remove(CacheKey);

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

            // Limpiar caché porque los datos han cambiado
            SimpleCacheManager.Instance.Remove(CacheKey);

            return new Response<CustomRolDTO>(true, "Rol actualizado correctamente", _mapper.Map<CustomRolDTO>(entity));
        }

        public async Task<Response<bool>> DeleteAsync(int id)
        {
            await _customRolRepository.DeleteAsync(id);

            // Limpiar caché porque los datos han cambiado
            SimpleCacheManager.Instance.Remove(CacheKey);

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
