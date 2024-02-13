using AutoMapper;
using Intranet.Models;
using Intranet.Repository.Entities;

namespace Intranet.AutoMapperProfiles
{
    public class IntranetAutoMapperProfile : Profile
    {
        public IntranetAutoMapperProfile()
        {
            CreateMap<UserEntity, UserViewModel>()
                .ForMember(uvm => uvm.Name, cfg => cfg.MapFrom(ue => ue.DisplayName));

            CreateMap<PhoneUserEntity, PhoneUserViewModel>();
        }
    }
}
