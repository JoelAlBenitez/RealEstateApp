using AutoMapper;
using RealEstateApp.Core.Application.DTOs.SaleType;
using RealEstateApp.Core.Application.ViewsModel.SaleType;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.SaleType
{
    public sealed class SaleTypeDtoToViewModelAndReverse : Profile
    {
        public SaleTypeDtoToViewModelAndReverse()
        {
            CreateMap<CreateSaleTypeViewModel, SaveSaleTypeDto>();
            CreateMap<EditSaleTypeViewModel, SaveSaleTypeDto>();
            CreateMap<SaleTypeDto, SaleTypeListViewModel>();
        }
    }
}
