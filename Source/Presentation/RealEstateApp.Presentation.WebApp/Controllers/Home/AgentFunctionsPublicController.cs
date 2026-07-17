using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Presentation.WebApp.Controllers.Home
{
    public class AgentFunctionsPublicController : Controller
    {
        private const int PageSize = 12;

        // HANDOFF: el repositorio pagina a 10 por defecto; se pide un lote amplio y se
        // pagina localmente a 12. Si el volumen de datos crece, cambiar a paginación
        // real con total de registros expuesto por el servicio.
        private const int ServiceFetchSize = 200;
        private const string HomeIndexView = "~/Views/Home/Index.cshtml";

        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApp _operationalAccountWebApp;
        private readonly IMapper _mapper;

        public AgentFunctionsPublicController(
            IPropertyService propertyService,
            IMapper mapper,
            IOperationalAccountWebApp operationalAccountWebApp
            )
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _operationalAccountWebApp = operationalAccountWebApp;
        }

        #region methods load
        public async Task<IActionResult> Agents()
        {
            return View(await BuildAgentsAsync(null));
        }

        public async Task<IActionResult> PropertyByAgent(string AgentId, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(AgentId))
            {
                TempData["Warning"] = "El agente solicitado no existe o no se encuentra disponible.";
                return View("Agents", await BuildAgentsAsync(null));
            }
            var result = await _propertyService.GetAvailableByAgentAsync(AgentId, 1, ServiceFetchSize);
            if (!result.IsValid)
            {
                AddErrors(result.Errors);
                return View("Agents", await BuildAgentsAsync(null));
            }
            var properties = _mapper.Map<IReadOnlyCollection<PropertyPublicViewModel>>(result.Value)
                ?? Array.Empty<PropertyPublicViewModel>();
            var vm = BuildAgentPropertiesViewModel(properties, page);
            vm.AgentId = AgentId;
            try
            {
                var agent = await _operationalAccountWebApp.GetConsultAgentById(AgentId);
                if (agent != null)
                {
                    vm.AgentName = $"{agent.Name} {agent.LastName}";
                }
            }
            catch
            {
                vm.AgentName = null;
            }
            return View(HomeIndexView, vm);
        }
        #endregion

        #region methods posts
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AgentsConsult(AgentConsultByNameOrLastNameViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Agents", await BuildAgentsAsync(null));
            }
            var map = _mapper.Map<ConsultAgentByNameOrLastNameDto>(vm);
            var result = await _operationalAccountWebApp.GetAgentByConsultCustomer(map);
            var agents = result == null
                ? Array.Empty<AgentViewModel>()
                : _mapper.Map<IReadOnlyCollection<AgentViewModel>>(result);
            if (agents == null || agents.Count == 0)
            {
                TempData["Info"] = "No se encontraron agentes activos con el nombre ingresado.";
            }
            return View("Agents", new AgentsHomeViewModel
            {
                Agents = agents ?? Array.Empty<AgentViewModel>(),
                Consult = vm
            });
        }
        #endregion

        #region private helpers
        private async Task<AgentsHomeViewModel> BuildAgentsAsync(AgentConsultByNameOrLastNameViewModel? consult)
        {
            var result = await _operationalAccountWebApp.GetAgentAllViewHomeByCustomer();
            var agents = result == null
                ? Array.Empty<AgentViewModel>()
                : _mapper.Map<IReadOnlyCollection<AgentViewModel>>(result);
            return new AgentsHomeViewModel
            {
                Agents = agents ?? Array.Empty<AgentViewModel>(),
                Consult = consult ?? new AgentConsultByNameOrLastNameViewModel { Name = "" }
            };
        }

        private HomePropertiesViewModel BuildAgentPropertiesViewModel(
            IReadOnlyCollection<PropertyPublicViewModel> properties,
            int page)
        {
            var totalPages = Math.Max(1, (int)Math.Ceiling(properties.Count / (double)PageSize));
            page = Math.Clamp(page, 1, totalPages);
            var items = properties.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            return new HomePropertiesViewModel
            {
                Properties = items,
                Filter = new PropertyFilterViewModel(),
                SearchByCode = new PropertySearchByCodeViewModel { Code = "" },
                Page = page,
                PageSize = PageSize,
                TotalPages = totalPages,
                IsAgentContext = true
            };
        }

        private void AddErrors(IReadOnlyCollection<Error> errors)
        {
            foreach (var item in errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }
        }
        #endregion
    }
}
