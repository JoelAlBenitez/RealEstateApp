using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.Password;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Auth
{
    public sealed class ResetPasswordDtoToViewModelAndReverse : Profile
    {
        public ResetPasswordDtoToViewModelAndReverse()
        {
            CreateMap<ResetPasswordDto, ResetPasswordViewModel>().ReverseMap();
        }
    }
}
