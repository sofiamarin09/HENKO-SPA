using AutoMapper;
using HankoSpa.Models;
using HankoSpa.DTOs;

namespace HankoSpa.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Servicio, ServiceDTO>().ReverseMap();
            CreateMap<Cita, CitaDTO>().ReverseMap();
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.CustomRolId, opt => opt.MapFrom(src => src.CustomRolId))
                .ForMember(dest => dest.CustomRol, opt => opt.MapFrom(src => src.CustomRol.NombreRol))
                .ReverseMap()
                .ForMember(dest => dest.CustomRol, opt => opt.Ignore());
            CreateMap<CustomRol, CustomRolDTO>();

        }

    }
}