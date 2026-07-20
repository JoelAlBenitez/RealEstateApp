using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Property
{
    public sealed class PropertyFilterDtoToViewModel : Profile
    {
        public PropertyFilterDtoToViewModel() {

            CreateMap<PropertyFilterDto, PropertyFilterViewModel>()
                .ForMember(opt => opt.IdTypeProperty, src => src.MapFrom(s => s.PropertyTypeId))
                .ForMember(opt => opt.TypePropery, src => src.Ignore())
                .ForMember(opt => opt.MaxPrice, src => src.MapFrom(s => s.MaxPrice))
                .ForMember(opt => opt.MinPrice, src => src.MapFrom(s => s.MinPrice))
                .ForMember(opt => opt.Bathrooms, src => src.MapFrom(s => s.Bathrooms))
                .ForMember(opt => opt.Bedrooms, src => src.MapFrom(s => s.Bedrooms))
                .ReverseMap();
        }
    }
}
