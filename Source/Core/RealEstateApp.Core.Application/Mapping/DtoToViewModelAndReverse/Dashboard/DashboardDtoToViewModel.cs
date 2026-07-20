using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Dashboard;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Dashboard
{
    public sealed class DashboardDtoToViewModel : Profile
    {
        public DashboardDtoToViewModel()
        {
            CreateMap<DashboardDto, DashboardViewModel>()
                .ForMember(dest => dest.ActiveAgents, opt => opt.MapFrom(src => src.ActiveAgentsCount))
                .ForMember(dest => dest.InactiveAgents, opt => opt.MapFrom(src => src.InactiveAgentsCount))
                .ForMember(dest => dest.ActiveClients, opt => opt.MapFrom(src => src.ActiveClientsCount))
                .ForMember(dest => dest.InactiveClients, opt => opt.MapFrom(src => src.InactiveClientsCount))
                .ForMember(dest => dest.ActiveDevelopers, opt => opt.MapFrom(src => src.ActiveDevelopersCount))
                .ForMember(dest => dest.InactiveDevelopers, opt => opt.MapFrom(src => src.InactiveDevelopersCount));
        }
    }
}
