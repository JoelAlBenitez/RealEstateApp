using AutoMapper;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.Properties
{
    public sealed class PropertyMappingProfile : Profile
    {
        public PropertyMappingProfile()
        {
            CreateMap<Property, PropertyDto>().ReverseMap();
            CreateMap<PropertyImage, PropertyImageDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ImageUrl))
                .ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Url));
            CreateMap<Property, SavePropertyDto>()
                .ForMember(dest => dest.ImprovementIds, opt => opt.MapFrom(src => src.PropertyImprovements != null ? src.PropertyImprovements.Select(pi => pi.ImprovementId).ToList() : new List<int>()))
                .ForMember(dest => dest.ExistingImageUrls, opt => opt.MapFrom(src => src.Images != null ? src.Images.Select(img => img.ImageUrl).ToList() : new List<string>()))
                .ReverseMap()
                .ForMember(dest => dest.PropertyImprovements, opt => opt.Ignore());
            CreateMap<PropertyFilterDto, PropertyFilterCriteria>().ReverseMap();
        }
    }
}
