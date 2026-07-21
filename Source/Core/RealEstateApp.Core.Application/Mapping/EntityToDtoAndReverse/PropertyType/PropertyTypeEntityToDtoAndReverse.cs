using AutoMapper;
using RealEstateApp.Core.Application.DTOs.PropertyType;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.PropertyType
{
    public sealed class PropertyTypeEntityToDtoAndReverse : Profile
    {
        public PropertyTypeEntityToDtoAndReverse()
        {
            CreateMap<RealEstateApp.Core.Domain.Entities.PropertyType, PropertyTypeDto>();
            CreateMap<RealEstateApp.Core.Domain.Entities.PropertyType, SavePropertyTypeDto>().ReverseMap();
        }
    }
}
