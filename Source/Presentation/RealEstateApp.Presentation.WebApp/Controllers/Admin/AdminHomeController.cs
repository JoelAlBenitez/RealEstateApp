using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Dashboard;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;

namespace RealEstateApp.Presentation.WebApp.Controllers.Admin
{
    [Authorize(Roles = "Administrador")]
    public class AdminHomeController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;

        public AdminHomeController(IDashboardService dashboardService, IMapper mapper)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool showActive = true)
        {
            var result = await _dashboardService.GetDashboardStatsAsync(showActive);
            
            ViewBag.ShowActive = showActive;

            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al cargar las estadísticas del sistema.";
                
                var fallbackVm = new DashboardViewModel
                {
                    AvailableProperties = 0,
                    SoldProperties = 0,
                    ActiveAgents = 0,
                    InactiveAgents = 0,
                    ActiveClients = 0,
                    InactiveClients = 0,
                    ActiveDevelopers = 0,
                    InactiveDevelopers = 0
                };
                
                return View(fallbackVm);
            }

            var viewModel = _mapper.Map<DashboardViewModel>(result.Value);
            return View(viewModel);
        }
    }
}
