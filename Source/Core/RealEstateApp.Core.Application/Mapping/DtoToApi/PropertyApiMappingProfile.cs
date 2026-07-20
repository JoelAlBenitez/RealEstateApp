using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Api.Properties;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;

namespace RealEstateApp.Core.Application.Mapping.DtoToApi
{
    public sealed class PropertyApiMappingProfile : Profile
    {
        public PropertyApiMappingProfile()
        {
            CreateMap<PropertyDto, PropertyApiDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.Status == PropertyState.Available ? "Disponible" : "Vendida"))
                .ForMember(dest => dest.PropertyType, opt => opt.Ignore())
                .ForMember(dest => dest.SaleType, opt => opt.Ignore())
                .ForMember(dest => dest.Improvements, opt => opt.Ignore());
        }
    }
}
