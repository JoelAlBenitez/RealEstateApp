using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Property
{
    public sealed class PropertyFilterDtoToViewModel : Profile
    {
        public PropertyFilterDtoToViewModel() {

            //agregar id de la propiedad cuando Sebastian lo descomente y Adrian mande los elementos de Tipos de propiedades

            CreateMap<PropertyFilterDto, PropertyFilterViewModel>()
                //modificar cuando sebastian termine y adrian para obtener este valor
                .ForMember(opt => opt.IdTypeProperty, src => src.Ignore())
                .ForMember(opt => opt.TypePropery, src => src.Ignore())
                .ForMember(opt => opt.MaxPrice, src => src.MapFrom(s => s.MaxPrice))
                .ForMember(opt => opt.MinPrice, src => src.MapFrom(s => s.MinPrice))
                .ForMember(opt => opt.Bathrooms, src => src.MapFrom(s => s.Bathrooms))
                .ForMember(opt => opt.Bedrooms, src => src.MapFrom(s => s.Bedrooms))
                .ReverseMap();
        }
    }
}
