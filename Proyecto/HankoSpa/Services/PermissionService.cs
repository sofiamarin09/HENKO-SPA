using AutoMapper;
using HankoSpa.DTOs;
using HankoSpa.Models;
using HankoSpa.Nucleo;
using HankoSpa.Repository;
using HankoSpa.Services.Interfaces;

namespace HankoSpa.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repo;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Response<List<PermissionDTO>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return new Response<List<PermissionDTO>>(true, "OK", _mapper.Map<List<PermissionDTO>>(list));
        }

        public async Task<Response<PermissionDTO>> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return new Response<PermissionDTO>(false, "Permiso no encontrado");
            return new Response<PermissionDTO>(true, "OK", _mapper.Map<PermissionDTO>(entity));
        }

        public async Task<Response<PermissionDTO>> CreateAsync(PermissionDTO dto)
        {
            var entity = _mapper.Map<Permission>(dto);
            await _repo.AddAsync(entity);
            return new Response<PermissionDTO>(true, "Permiso creado correctamente", _mapper.Map<PermissionDTO>(entity));
        }

        public async Task<Response<PermissionDTO>> UpdateAsync(PermissionDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.PermisoId);
            if (entity == null)
                return new Response<PermissionDTO>(false, "Permiso no encontrado");
            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
            return new Response<PermissionDTO>(true, "Permiso actualizado correctamente", _mapper.Map<PermissionDTO>(entity));
        }

        public async Task<Response<bool>> DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            return new Response<bool>(true, "Permiso eliminado correctamente", true);
        }
    }
}

