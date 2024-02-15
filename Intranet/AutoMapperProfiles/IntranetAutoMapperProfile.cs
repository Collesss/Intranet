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

            CreateMap<PhoneUserEntity, PhoneUserViewModel>()
                .ForMember(puvm => puvm.Type, cfg => cfg.AddTransform(type => TransformTypePhone(type)));
        }

        private string TransformTypePhone(string typePhone)
        {
            Dictionary<string, string> transformDict = new Dictionary<string, string>()
            {
                ["telephonenumber"] = "Основной номер",
                ["othertelephone"] = "Дополнительные номера",
                ["ipphone"] = "Ip телефон",
                ["otheripphone"] = "Дополнительные Ip телефоны"
            };

            return transformDict.TryGetValue(typePhone, out string transformedType) ? transformedType : typePhone;
        }
    }
}
