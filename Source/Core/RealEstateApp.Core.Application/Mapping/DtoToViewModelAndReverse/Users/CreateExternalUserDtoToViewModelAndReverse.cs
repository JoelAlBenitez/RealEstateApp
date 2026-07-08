using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users
{
    public sealed  class CreateExternalUserDtoToViewModelAndReverse : Profile
    {
        public CreateExternalUserDtoToViewModelAndReverse()
        {
            CreateMap<RegisterExternalUsers, CreateExternalUserViewModel>().ReverseMap()
                .ForMember(opt => opt.Origin, des => des.Ignore());
                
        }
    }
}
