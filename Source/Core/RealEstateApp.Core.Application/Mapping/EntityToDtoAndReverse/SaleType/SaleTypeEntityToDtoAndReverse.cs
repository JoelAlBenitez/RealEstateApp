using AutoMapper;
using RealEstateApp.Core.Application.DTOs.SaleType;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.SaleType
{
    public sealed class SaleTypeEntityToDtoAndReverse : Profile
    {
        public SaleTypeEntityToDtoAndReverse()
        {
            CreateMap<RealEstateApp.Core.Domain.Entities.SaleType, SaleTypeDto>();
            CreateMap<RealEstateApp.Core.Domain.Entities.SaleType, SaveSaleTypeDto>().ReverseMap();
        }
    }
}
