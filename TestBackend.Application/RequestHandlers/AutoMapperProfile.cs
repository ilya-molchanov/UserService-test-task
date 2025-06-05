using AutoMapper;
using TestBackend.Internal.BusinessObjects;
using TestBackend.Internal.Enums;
using TestBackend.ServiceLibrary.Models;

namespace TestBackend.Application.RequestHandlers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateUserDto, SqlUser>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(e => e.Name))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(e => e.Email))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(e => BCrypt.Net.BCrypt.HashPassword(e.Password)))
                .ForMember(dto => dto.Role, opt => opt.MapFrom(e => (int)e.Role));

            CreateMap<SqlUser, UserDto>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(e => e.Name))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(e => e.Email))
                .ForMember(dto => dto.Role, opt => opt.MapFrom(e => (UserRoleCode)e.Role));
        }
    }
}
