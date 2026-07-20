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
        private const string HomeIndexView = "~/Views/Home/Index.cshtml";

        private readonly IPropertyQueryService _propertyQueryService;
        private readonly IOperationalAccountWebApp _operationalAccountWebApp;
        private readonly IMapper _mapper;

        public AgentFunctionsPublicController(
            IPropertyQueryService propertyQueryService,
            IMapper mapper,
            IOperationalAccountWebApp operationalAccountWebApp
            )
        {
            _propertyQueryService = propertyQueryService;
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
            var agent = await FindAgentAsync(AgentId);
            if (agent == null)
            {
                TempData["Warning"] = "El agente solicitado no existe o no se encuentra disponible.";
                return View("Agents", await BuildAgentsAsync(null));
            }

            var totalPages = 1;
            var countResult = await _propertyQueryService.CountAvailableByAgentAsync(AgentId);
            if (countResult.IsValid)
            {
                totalPages = Math.Max(1, (int)Math.Ceiling(countResult.Value / (double)PageSize));
            }
            page = Math.Clamp(page, 1, totalPages);

            var result = await _propertyQueryService.GetAvailableByAgentAsync(AgentId, page, PageSize);
            if (!result.IsValid)
            {
                AddErrors(result.Errors);
                return View("Agents", await BuildAgentsAsync(null));
            }
            var properties = _mapper.Map<IReadOnlyCollection<PropertyPublicViewModel>>(result.Value)
                ?? Array.Empty<PropertyPublicViewModel>();
            var vm = BuildAgentPropertiesViewModel(properties, page, totalPages);
            vm.AgentId = AgentId;
            vm.AgentName = $"{agent.Name} {agent.LastName}";
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
            return View("Agents", new AgentsHomeViewModel
            {
                Agents = agents ?? Array.Empty<AgentViewModel>(),
                Consult = vm,
                IsConsult = true
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

        private async Task<ConsultAgentDto?> FindAgentAsync(string agentId)
        {
            if (string.IsNullOrWhiteSpace(agentId))
            {
                return null;
            }
            try
            {
                return await _operationalAccountWebApp.GetConsultAgentById(agentId);
            }
            catch
            {
                return null;
            }
        }

        private HomePropertiesViewModel BuildAgentPropertiesViewModel(
            IReadOnlyCollection<PropertyPublicViewModel> properties,
            int page,
            int totalPages)
        {
            return new HomePropertiesViewModel
            {
                Properties = properties,
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
