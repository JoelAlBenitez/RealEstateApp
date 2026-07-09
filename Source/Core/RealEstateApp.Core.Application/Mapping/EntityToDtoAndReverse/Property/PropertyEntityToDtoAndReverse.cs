using AutoMapper;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.Property
{
    public sealed class PropertyEntityToDtoAndReverse : Profile
    {
        public PropertyEntityToDtoAndReverse()
        {
            CreateMap<Domain.Entities.Property, PropertyDto>().ReverseMap();
            CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();
            CreateMap<Domain.Entities.Property, SavePropertyDto>().ReverseMap();
            CreateMap<PropertyFilterDto, PropertyFilterCriteria>().ReverseMap();
        }
    }
}
