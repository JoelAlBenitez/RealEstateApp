using AutoMapper;
using RealEstateApp.Core.Application.DTOs.PropertyType;
using RealEstateApp.Core.Application.ViewsModel.PropertyType;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.PropertyType
{
    public sealed class PropertyTypeDtoToViewModelAndReverse : Profile
    {
        public PropertyTypeDtoToViewModelAndReverse()
        {
            CreateMap<CreatePropertyTypeViewModel, SavePropertyTypeDto>();
            CreateMap<EditPropertyTypeViewModel, SavePropertyTypeDto>();
            CreateMap<PropertyTypeDto, PropertyTypeListViewModel>();
        }
    }
}
