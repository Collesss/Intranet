using AutoMapper;
using Intranet.Models;
using Intranet.Models.PageItems;
using Intranet.Models.PageItems.Dto;
using Intranet.Repository.Entities;

namespace Intranet.AutoMapperProfiles
{
    public class IntranetAutoMapperProfile : Profile
    {
        public IntranetAutoMapperProfile()
        {
            CreateMap<ItemsDto, Query>();
            CreateMap<PageItemsDto, Page>();
            CreateMap<SortItemsDto, Sort>();
            CreateMap<FilterItemsDto, Filter>();


            CreateMap<PageResult<UserEntity>, PageItemsViewModel<UserViewModel>>();


            CreateMap<UserEntity, UserViewModel>()
                .ForMember(uvm => uvm.Name, cfg => cfg.MapFrom(ue => ue.DisplayName))
                .ForMember(uvm => uvm.Phones, cfg => cfg.MapFrom(ue => ue.Phones
                    .GroupBy(phone => phone.Type, phone => phone.PhoneNumbers, (type, numbers) => new PhoneUserViewModel() { Type = TransTypePhone(type), PhoneNumbers = numbers.ToArray() })));


            /*
            CreateMap<PhoneEntity, PhoneUserViewModel>()
                .ForMember(puvm => puvm.Type, cfg => cfg.AddTransform(type => TransTypePhone(type)));
            */
        }

        private string TransTypePhone(string typePhone)
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
