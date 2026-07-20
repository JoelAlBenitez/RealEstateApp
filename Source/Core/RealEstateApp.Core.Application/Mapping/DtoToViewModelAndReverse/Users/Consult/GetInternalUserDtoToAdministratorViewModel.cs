using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Consult
{
    public sealed class GetInternalUserDtoToAdministratorViewModel : Profile
    {
        public GetInternalUserDtoToAdministratorViewModel()
        {
            CreateMap<GetInternalUserDto, AdministratorListItemViewModel>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.IdCard, opt => opt.MapFrom(src => src.IDCard));
        }
    }
}
