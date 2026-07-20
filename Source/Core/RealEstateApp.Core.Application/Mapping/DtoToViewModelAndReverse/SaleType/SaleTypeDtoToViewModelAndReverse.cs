using AutoMapper;
using RealEstateApp.Core.Application.DTOs.SaleType;
using RealEstateApp.Core.Application.ViewsModel.SaleType;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.SaleType
{
    public sealed class SaleTypeDtoToViewModelAndReverse : Profile
    {
        public SaleTypeDtoToViewModelAndReverse()
        {
            CreateMap<SaveSaleTypeViewModel, SaveSaleTypeDto>()
                .ReverseMap();

            CreateMap<SaleTypeDto, SaleTypeViewModel>()
                .ReverseMap();
        }
    }
}
