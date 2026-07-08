using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.InternaUser;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users
{
    public sealed class CreateInternalUserDtoToViewModelAndReverse : Profile
    {
        public CreateInternalUserDtoToViewModelAndReverse()
        {
            CreateMap<RegisterInternalUsersDto, CreateInternalUserViewModel>().ReverseMap();
        }
    }
}
