using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.ViewsModel.Improvement;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Improvement
{
    public sealed class ImprovementDtoToViewModelAndReverse : Profile
    {
        public ImprovementDtoToViewModelAndReverse()
        {
            CreateMap<SaveImprovementViewModel, SaveImprovementDto>()
                .ReverseMap();
                
            CreateMap<ImprovementDto, ImprovementViewModel>()
                .ReverseMap();
        }
    }
}
