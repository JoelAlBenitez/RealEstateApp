using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.ViewsModel.Users.Auth;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Auth
{
    public sealed class LoginUserDtoToViewModelAndReverse : Profile
    {
        public LoginUserDtoToViewModelAndReverse() {
            CreateMap<LoginDto, LoginUserViewModel>().ReverseMap();
        }
    }
}
