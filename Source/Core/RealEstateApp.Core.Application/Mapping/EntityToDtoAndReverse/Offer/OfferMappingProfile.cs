using AutoMapper;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.DTOs.Offer;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.Offer
{
    public sealed class OfferMappingProfile : Profile
    {
        public OfferMappingProfile()
        {
            CreateMap<Domain.Entities.Offer, OfferDto>().ReverseMap();
            CreateMap<Domain.Entities.Offer, SaveOfferDto>().ReverseMap();
        }
    }
}
