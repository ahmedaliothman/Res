using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ResidencyApplication.Services.Models.CustomReturnTypes;
using ResidencyApplication.Services.Models.EntityModels;

namespace ResidencyApplication.Services.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<UserApplication, UserApplicationsDTO > ().ReverseMap(); // means you want to map from User to UserDTO
            CreateMap<object, UserApplicationsDetailedDTO> ().ReverseMap(); // means you want to map from User to UserDTO

            CreateMap<UserApplicationsLog, UserApplicationsLogDTO>().ReverseMap(); // means you want to map from Userlog to UserLogDTO

            CreateMap<Nationality, NationalitiesDTO> ().ReverseMap();

            CreateMap<Nationality, LookUpsReturnObj> ().
                ForMember(dist=> dist.label, s=>s.MapFrom(x=>x.NationalityName)).
                ForMember(dist=> dist.value, s=>s.MapFrom(x=>x.NationalityId)).
                ReverseMap(); // means you want to map from User to UserDTO

            CreateMap<UserRole, LookUpsReturnObj>().
          ForMember(dist => dist.label, s => s.MapFrom(x => x.UserRoleNameAr)).
          ForMember(dist => dist.value, s => s.MapFrom(x => x.UserRoleId)).
          ReverseMap();
            CreateMap<UserType, LookUpsReturnObj>().
           ForMember(dist => dist.label, s => s.MapFrom(x => x.UserTypeName)).
           ForMember(dist => dist.value, s => s.MapFrom(x => x.UserTypeId)).
           ReverseMap(); // means you want to map from User to UserDTO 
           
            CreateMap<ApplicationType, LookUpsReturnObj>().
           ForMember(dist => dist.label, s => s.MapFrom(x => x.ApplicationTypeName)).
           ForMember(dist => dist.value, s => s.MapFrom(x => x.ApplicationTypeId)).
           ReverseMap(); // means you want to map from User to UserDTO

            CreateMap<Organization, LookUpsReturnObj>().
                ForMember(dis => dis.label, s => s.MapFrom(x => x.Name)).
                ForMember(dist => dist.value, s => s.MapFrom(x => x.Id)).ReverseMap();

            CreateMap<ApplicationType, ApplicationTypesDTO>().ReverseMap();

        }
    }
}
