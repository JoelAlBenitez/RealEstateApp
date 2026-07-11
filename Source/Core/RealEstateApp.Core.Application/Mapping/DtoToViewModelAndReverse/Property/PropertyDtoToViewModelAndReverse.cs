using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Property
{
    public sealed class PropertyDtoToViewModelAndReverse : Profile
    {
        public PropertyDtoToViewModelAndReverse()
        {
            CreateMap<PropertyDto, PropertyCardViewModel>().ReverseMap();
            CreateMap<PropertyDto, PropertyDetailViewModel>().ReverseMap();
            CreateMap<SavePropertyDto, SavePropertyViewModel>().ReverseMap();
        }
    }
}
