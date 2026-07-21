using AutoMapper;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.FavoriteProperty
{
    public sealed class FavoritePropertyMappingProfile : Profile
    {
        public FavoritePropertyMappingProfile()
        {
            CreateMap<Domain.Entities.FavoriteProperty, FavoritePropertyDto>().ReverseMap();
            CreateMap<Domain.Entities.FavoriteProperty, SaveFavoritePropertyDto>().ReverseMap();
        }
    }
}
