using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Dashboard;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Dashboard
{
    public sealed class DashboardDtoToViewModel : Profile
    {
        public DashboardDtoToViewModel()
        {
            // El DTO trae un único conteo por tipo de usuario (activos u inactivos según ShowingActive),
            // por eso se enruta ese valor al campo correspondiente y el opuesto queda en 0 (no se muestra).
            CreateMap<DashboardDto, DashboardViewModel>()
                .ForMember(d => d.ActiveAgents, o => o.MapFrom(s => s.ShowingActive ? s.AgentsCount : 0))
                .ForMember(d => d.InactiveAgents, o => o.MapFrom(s => s.ShowingActive ? 0 : s.AgentsCount))
                .ForMember(d => d.ActiveClients, o => o.MapFrom(s => s.ShowingActive ? s.ClientsCount : 0))
                .ForMember(d => d.InactiveClients, o => o.MapFrom(s => s.ShowingActive ? 0 : s.ClientsCount))
                .ForMember(d => d.ActiveDevelopers, o => o.MapFrom(s => s.ShowingActive ? s.DevelopersCount : 0))
                .ForMember(d => d.InactiveDevelopers, o => o.MapFrom(s => s.ShowingActive ? 0 : s.DevelopersCount))
                .ForMember(d => d.ShowActive, o => o.MapFrom(s => s.ShowingActive));
        }
    }
}
