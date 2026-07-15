using AutoMapper;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Application.ViewsModel.FavoriteProperty;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.FavoriteProperty
{
    public sealed class FavoritePropertyDtoToViewModelAndReverse : Profile
    {
        public FavoritePropertyDtoToViewModelAndReverse()
        {
            CreateMap<FavoritePropertyDto, FavoritePropertyViewModel>().ReverseMap();
            CreateMap<SaveFavoritePropertyDto, FavoritePropertyViewModel>().ReverseMap();
        }
    }
}
