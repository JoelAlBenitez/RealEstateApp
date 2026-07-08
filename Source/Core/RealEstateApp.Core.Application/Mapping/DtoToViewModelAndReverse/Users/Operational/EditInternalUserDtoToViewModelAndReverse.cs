using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.InternaUser;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Operational
{
    public sealed class EditInternalUserDtoToViewModelAndReverse : Profile
    {
        public EditInternalUserDtoToViewModelAndReverse() {
            CreateMap<EditInernalUserDto, EditInternalUserViewModel>().ReverseMap();
        }
    }
}
