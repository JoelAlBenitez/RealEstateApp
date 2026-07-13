using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Operational
{
    public sealed class EditExternalUserDtoToViewModelAndReverse : Profile
    {
        public EditExternalUserDtoToViewModelAndReverse()
        {
            CreateMap<EditAgentUserDto, EditExternalUserViewModel>().ReverseMap()
               .ForMember(opt => opt.ProfileImg, des => des.MapFrom(src => src.ProfileImgCurrent))
               .ForMember(opt => opt.ChangePorfileImg, des => des.Ignore())
               .ForMember(opt => opt.ProfileImgUrl, des => des.Ignore());
               

        }
    }
}
