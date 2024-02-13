using AutoMapper;
using Intranet.Repository.ActiveDirectory.Extenstions;
using Intranet.Repository.Entities;
using System.DirectoryServices;

namespace Intranet.Repository.ActiveDirectory.AutoMapperProfiles
{
    public class ActiveDirectoryAutoMapperProfile : Profile
    {
        public ActiveDirectoryAutoMapperProfile() 
        {
            CreateMap<SearchResult, UserEntity>()
                .ForMember(user => user.Id, cfg => cfg.MapFrom(sr => sr.GetStrSID()))
                .ForMember(user => user.DisplayName, cfg => cfg.MapFrom(sr => sr.GetStrFirstProp("displayName")))
                .ForMember(user => user.Email, cfg => cfg.MapFrom(sr => sr.GetStrFirstProp("mail")))
                .ForMember(user => user.UserPrincipalName, cfg => cfg.MapFrom(sr => sr.GetStrFirstProp("userprincipalname")))
                .ForMember(user => user.Phones, cfg => cfg.MapFrom(sr => GetPhones(sr)));
        }

        private PhoneUserEntity[] GetPhones(SearchResult searchResult)
        {
            string[] typePhones = new string[] 
            {
                "telephonenumber",
                "othertelephone",
                "ipphone"
            };

            return typePhones
                .SelectMany(typePhone => searchResult.GetArrStrProp(typePhone)
                .Select(phone => new PhoneUserEntity { Type = typePhone, PhoneNumber = phone}))
                .ToArray();
        }
    }
}
