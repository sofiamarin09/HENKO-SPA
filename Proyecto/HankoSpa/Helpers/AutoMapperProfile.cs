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
            CreateMap<CustomRol, CustomRolDTO>().ReverseMap(); // <-- Modificado aquí
            CreateMap<Permission, PermissionDTO>().ReverseMap();
            CreateMap<RolPermission, RolPermissionDTO>()
                .ForMember(dest => dest.NombreRol, opt => opt.MapFrom(src => src.CustomRol != null ? src.CustomRol.NombreRol : null))
                .ForMember(dest => dest.NombrePermiso, opt => opt.MapFrom(src => src.Permission != null ? src.Permission.NombrePermiso : null))
                .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Permission != null ? src.Permission.Module : null))
                .ReverseMap()
                .ForMember(dest => dest.CustomRol, opt => opt.Ignore())
                .ForMember(dest => dest.Permission, opt => opt.Ignore());
        }
    }
}
