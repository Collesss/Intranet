using AutoMapper;
using Intranet.Api.Db.Entities;
using Intranet.Api.Dto.User.Response;

namespace Intranet.Api.AutoMapperProfiles
{
    public class IntranetApiAutoMapperProfile : Profile
    {
        public IntranetApiAutoMapperProfile() 
        {
            CreateMap<UserEntity, UserResponseDto>();
        }
    }
}
