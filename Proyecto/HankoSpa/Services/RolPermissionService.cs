using AutoMapper;
using HankoSpa.DTOs;
using HankoSpa.Models;
using HankoSpa.Nucleo;
using HankoSpa.Repository;
using HankoSpa.Services.Interfaces;

namespace HankoSpa.Services
{
    public class RolPermissionService : IRolPermissionService
    {
        private readonly IRolPermissionRepository _repo;
        private readonly IMapper _mapper;

        public RolPermissionService(IRolPermissionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Response<List<RolPermissionDTO>>> GetPermissionsByRolIdAsync(int rolId)
        {
            var list = await _repo.GetByRolIdAsync(rolId);
            return new Response<List<RolPermissionDTO>>(true, "OK", _mapper.Map<List<RolPermissionDTO>>(list));
        }

        public async Task<Response<bool>> AssignPermissionAsync(RolPermissionDTO dto)
        {
            var entity = _mapper.Map<RolPermission>(dto);
            await _repo.AddAsync(entity);
            return new Response<bool>(true, "Permiso asignado correctamente", true);
        }

        public async Task<Response<bool>> RemovePermissionAsync(int rolId, int permissionId)
        {
            await _repo.DeleteAsync(rolId, permissionId);
            return new Response<bool>(true, "Permiso eliminado correctamente", true);
        }
    }
}
