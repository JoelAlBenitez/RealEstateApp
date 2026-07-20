using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Dashboard;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Dashboard
{
    public sealed class DashboardDtoToViewModel : Profile
    {
        public DashboardDtoToViewModel()
        {
            CreateMap<DashboardDto, DashboardViewModel>();
        }
    }
}
