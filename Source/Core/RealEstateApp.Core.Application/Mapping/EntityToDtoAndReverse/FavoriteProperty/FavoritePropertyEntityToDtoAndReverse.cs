using AutoMapper;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.FavoriteProperty
{
    public sealed class FavoritePropertyEntityToDtoAndReverse : Profile
    {
        public FavoritePropertyEntityToDtoAndReverse()
        {
            CreateMap<Domain.Entities.FavoriteProperty, FavoritePropertyDto>().ReverseMap();
            CreateMap<Domain.Entities.FavoriteProperty, SaveFavoritePropertyDto>().ReverseMap();
        }
    }
}
