using AutoMapper;
using RealEstateApp.Core.Application.DTOs.PropertyType;
using RealEstateApp.Core.Application.ViewsModel.PropertyType;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.PropertyType
{
    public sealed class PropertyTypeDtoToViewModelAndReverse : Profile
    {
        public PropertyTypeDtoToViewModelAndReverse()
        {
            CreateMap<SavePropertyTypeViewModel, SavePropertyTypeDto>()
                .ReverseMap();
                
            CreateMap<PropertyTypeDto, PropertyTypeViewModel>()
                .ReverseMap();
        }
    }
}
