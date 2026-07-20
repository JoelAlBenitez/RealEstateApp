using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Improvement;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.Improvement
{
    public sealed class ImprovementEntityToDtoAndReverse : Profile
    {
        public ImprovementEntityToDtoAndReverse()
        {
            CreateMap<RealEstateApp.Core.Domain.Entities.Improvement, ImprovementDto>();
            CreateMap<RealEstateApp.Core.Domain.Entities.Improvement, SaveImprovementDto>().ReverseMap();
        }
    }
}
