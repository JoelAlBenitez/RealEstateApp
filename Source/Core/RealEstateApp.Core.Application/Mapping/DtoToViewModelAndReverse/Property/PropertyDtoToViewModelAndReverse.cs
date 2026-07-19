using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Property
{
    public sealed class PropertyDtoToViewModelAndReverse : Profile
    {
        public PropertyDtoToViewModelAndReverse()
        {
            CreateMap<PropertyDto, PropertyCardViewModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images != null && src.Images.Any() ? src.Images.OrderBy(i => i.Id).First().Url : null))
                .ReverseMap();

            CreateMap<PropertyDto, PropertyDetailViewModel>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images != null ? src.Images.OrderBy(i => i.Id).Select(i => i.Url).ToList() : new List<string>()))
                .ReverseMap();

            CreateMap<SavePropertyDto, SavePropertyViewModel>().ReverseMap();
        }
    }
}
