using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Application.ViewsModel.Offer;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Offer
{
    public sealed class OfferDtoToViewModelAndReverse : Profile
    {
        public OfferDtoToViewModelAndReverse()
        {
            CreateMap<OfferDto, OfferViewModel>().ReverseMap();
            CreateMap<SaveOfferDto, CreateOfferViewModel>().ReverseMap();
        }
    }
}
