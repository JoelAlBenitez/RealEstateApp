using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Presentation.WebApp.Controllers.Home
{
    public class HomeController : Controller
    {
        private const int PageSize = 12;

        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApp _operationalAccountWebApp;
        private readonly IMapper _mapper;

        public HomeController(
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

        #region methods properties public
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await BuildHomeAsync(null, page));
        }

        public async Task<IActionResult> DetailtsProperty(int IdProperty)
        {
            var result = await _propertyService.GetByIdWithDetailsAsync(IdProperty);
            if (!result.IsValid)
            {
                AddErrors(result.Errors);
                return View("Index", await BuildHomeAsync(null, 1));
            }
            var map = _mapper.Map<PropertyDetailViewModel>(result.Value);
            return View("DetailsProperty", map);
        }
        #endregion

        #region methods agents public
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
            var result = await _propertyService.GetAvailableByAgentAsync(AgentId);
            if (!result.IsValid)
            {
                AddErrors(result.Errors);
                return View("Agents", await BuildAgentsAsync(null));
            }
            var properties = _mapper.Map<IReadOnlyCollection<PropertyPublicViewModel>>(result.Value)
                ?? Array.Empty<PropertyPublicViewModel>();
            var vm = BuildHomeViewModel(properties, new PropertyFilterViewModel(), page, true);
            vm.IsAgentContext = true;
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
            return View("Index", vm);
        }
        #endregion

        #endregion

        #region methods posts
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ConsultByCode(PropertySearchByCodeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await BuildHomeAsync(null, 1));
            }
            var consult = await _propertyService.GetByCodeAsync(vm.Code);
            if (!consult.IsValid || consult.Value == null)
            {
                AddErrors(consult.Errors);
                return View("Index", await BuildHomeAsync(null, 1));
            }
            return RedirectToAction(nameof(DetailtsProperty), new { IdProperty = consult.Value.Id });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> FilterProperty(PropertyFilterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "La busqueda no pudo ser realizada. Favor revise los valores ingresados.");
                return View("Index", await BuildHomeAsync(null, 1));
            }
            var filters = _mapper.Map<PropertyFilterDto>(vm);
            var result = await BuildHomeAsync(filters, 1, false);
            result.Filter = vm;
            result.IsFiltered = true;
            return View("Index", result);
        }

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
        private async Task<HomePropertiesViewModel> BuildHomeAsync(PropertyFilterDto? filters, int page, bool paginateServerSide = true)
        {
            var result = await _propertyService.GetAvailableAsync(filters);
            IReadOnlyCollection<PropertyPublicViewModel> properties = Array.Empty<PropertyPublicViewModel>();
            if (!result.IsValid)
            {
                AddErrors(result.Errors);
            }
            else
            {
                properties = _mapper.Map<IReadOnlyCollection<PropertyPublicViewModel>>(result.Value)
                    ?? Array.Empty<PropertyPublicViewModel>();
            }
            return BuildHomeViewModel(properties, new PropertyFilterViewModel(), page, paginateServerSide);
        }

        private static HomePropertiesViewModel BuildHomeViewModel(
            IReadOnlyCollection<PropertyPublicViewModel> properties,
            PropertyFilterViewModel filter,
            int page,
            bool paginateServerSide)
        {
            // HANDOFF: cuando el otro dev entregue el servicio de tipos de propiedad,
            // cargar aquí filter.TypePropery con el listado de TypePropertyViewModel para
            // que el select del formulario de filtro (_PropertyFilter) se llene solo.
            var totalPages = Math.Max(1, (int)Math.Ceiling(properties.Count / (double)PageSize));
            if (!paginateServerSide)
            {
                return new HomePropertiesViewModel
                {
                    Properties = properties,
                    Filter = filter,
                    SearchByCode = new PropertySearchByCodeViewModel { Code = "" },
                    Page = 1,
                    PageSize = PageSize,
                    TotalPages = 1
                };
            }
            page = Math.Clamp(page, 1, totalPages);
            var items = properties.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            return new HomePropertiesViewModel
            {
                Properties = items,
                Filter = filter,
                SearchByCode = new PropertySearchByCodeViewModel { Code = "" },
                Page = page,
                PageSize = PageSize,
                TotalPages = totalPages
            };
        }

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
