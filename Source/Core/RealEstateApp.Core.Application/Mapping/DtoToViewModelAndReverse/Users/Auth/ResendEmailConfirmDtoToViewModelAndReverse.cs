using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Auth
{
    public sealed class ResendEmailConfirmDtoToViewModelAndReverse : Profile
    {
        public ResendEmailConfirmDtoToViewModelAndReverse()
        {
            CreateMap<ResendActivationEmailDto, ResendActivationEmailViewModel>().ReverseMap();

        }
    }
}
