using AutoMapper;
using HankoSpa.DTOs;
using HankoSpa.Nucleo;
using HankoSpa.Repository;
using HankoSpa.Services.Interfaces;

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
            var response = new Response<List<CustomRolDTO>>();

            var customRoles = await _customRolRepository.GetAllAsync();
            var dtoList = _mapper.Map<List<CustomRolDTO>>(customRoles);
            return new Response<List<CustomRolDTO>>(true, "Roles obtenidos correctamente.", dtoList);

        }


    }
}