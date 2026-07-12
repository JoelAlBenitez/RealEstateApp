using AutoMapper;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.Properties
{
    public sealed class PropertyEntityToDtoAndReverse : Profile
    {
        public PropertyEntityToDtoAndReverse()
        {
            CreateMap<Property, PropertyDto>().ReverseMap();
            CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();
            CreateMap<Property, SavePropertyDto>().ReverseMap();
            CreateMap<PropertyFilterDto, PropertyFilterCriteria>().ReverseMap();
        }
    }
}
