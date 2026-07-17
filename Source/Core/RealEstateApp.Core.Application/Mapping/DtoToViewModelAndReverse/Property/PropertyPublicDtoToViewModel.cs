using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Property
{
    public sealed class PropertyPublicDtoToViewModel : Profile
    {
        // HANDOFF: imagen por defecto usada cuando la propiedad no tiene imágenes cargadas.
        private const string DefaultPropertyImage = "/resources/banner.jpg";

        // HANDOFF: PropertyDto aún no expone SaleTypeName (comentado por límites de módulo).
        // Cuando el otro dev habilite SaleTypeName en PropertyDto, mapear TypeSale desde
        // s.SaleTypeName y eliminar este placeholder.
        private const string TypeSalePlaceholder = "En venta";

        public PropertyPublicDtoToViewModel() {
            CreateMap<PropertyDto, PropertyPublicViewModel>()
                .ForMember(opt => opt.Id, src => src.MapFrom(s => s.Id))
                .ForMember(
                opt => opt.FirtsImage,
                src =>  src.MapFrom(
                    s => s.Images != null && s.Images.Count > 0 && s.Images.First().Url != null
                    ? s.Images.First().Url : DefaultPropertyImage
                    )
                )
                .ForMember(opt => opt.TypeSale, src => src.MapFrom(s => TypeSalePlaceholder))
                .ForMember(opt => opt.NumberOfBathrooms, src => src.MapFrom(s => s.Bathrooms))
                .ForMember(opt => opt.NumberOfBedrooms, src => src.MapFrom(s => s.Bedrooms))
                .ForMember(opt => opt.Price, src => src.MapFrom(s => s.Price))
                .ForMember(opt => opt.Code, src => src.MapFrom(s => s.Code))
                .ForMember(opt => opt.Size, src => src.MapFrom(s => s.Size));
        }
    }
}
