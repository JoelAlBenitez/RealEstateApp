using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.InternaUser;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Operational
{
    public sealed class EditInternalUserDtoToViewModelAndReverse : Profile
    {
        public EditInternalUserDtoToViewModelAndReverse() {
            CreateMap<EditInternalUserDto, EditInternalUserViewModel>().ReverseMap();

            CreateMap<GetInternalUserDto, EditInternalUserViewModel>()
                .ForMember(dest => dest.IdCard, opt => opt.MapFrom(src => src.IDCard))
                .ForMember(dest => dest.NewPassword, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmNewPassword, opt => opt.Ignore());
        }
    }
}
