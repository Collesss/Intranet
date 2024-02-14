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
                .ForMember(uvm => uvm.Name, cfg => cfg.MapFrom(ue => ue.DisplayName))
                .ForMember(uvm => uvm.PhonesNumer, cfg => cfg.MapFrom(ue => TransfromTypePhoneNumber(ue.PhonesNumer)));

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

        private Dictionary<string, string[]> TransfromTypePhoneNumber(Dictionary<string, string[]> transfromPhone)
        {
            Dictionary<string, string> transformDict = new Dictionary<string, string>()
            {
                ["telephonenumber"] = "Основной номер",
                ["othertelephone"] = "Дополнительные номера",
                ["ipphone"] = "Ip телефон",
                ["otheripphone"] = "Дополнительные Ip телефоны"
            };

            string Transform(string typePhone) =>
                transformDict.TryGetValue(typePhone, out string transformedType) ? transformedType : typePhone;

            return new Dictionary<string, string[]>(transfromPhone.Select(kvp => KeyValuePair.Create(Transform(kvp.Key), kvp.Value)));
        }
    }
}
