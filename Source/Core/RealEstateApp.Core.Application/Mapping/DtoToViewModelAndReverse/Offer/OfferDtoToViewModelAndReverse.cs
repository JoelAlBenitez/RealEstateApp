using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Application.ViewsModel.Offer;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Offer
{
    public sealed class OfferDtoToViewModelAndReverse : Profile
    {
        public OfferDtoToViewModelAndReverse()
        {
            CreateMap<OfferDto, OfferViewModel>()
                .ForMember(dest => dest.PropertyCode, opt => opt.MapFrom(src => src.Property != null ? src.Property.Code : null))
                .ReverseMap();

            CreateMap<SaveOfferDto, CreateOfferViewModel>().ReverseMap();
        }
    }
}
