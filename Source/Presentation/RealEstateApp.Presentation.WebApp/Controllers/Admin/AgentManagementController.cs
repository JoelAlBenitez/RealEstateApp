using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Agent;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;

namespace RealEstateApp.Presentation.WebApp.Controllers.Admin
{
    [Authorize(Roles = "Administrador")]
    public class AgentManagementController : Controller
    {
        private const int PageSizeValue = 10;

        private readonly IAgentManagementService _agentManagementService;
        private readonly IMapper _mapper;

        public AgentManagementController(
            IAgentManagementService agentManagementService,
            IMapper mapper)
        {
            _agentManagementService = agentManagementService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(AgentListFilter filter = AgentListFilter.Todos, int page = 1)
        {
            var allResult = await _agentManagementService.GetAgentsAsync();
            if (!allResult.IsValid)
            {
                TempData["ErrorMessage"] = allResult.Errors.FirstOrDefault()?.Description
                    ?? "Ocurrió un error al cargar el listado de agentes.";
                return View(new AgentManagementIndexViewModel { Filter = filter });
            }

            var allAgents = allResult.Value;

            var pendingResult = await _agentManagementService.GetPendingConfirmationAgentsAsync();
            var pendingAgents = pendingResult.IsValid
                ? pendingResult.Value
                : new List<AdminConsultAgentDto>();

            var countAll = allAgents!.Count;
            var countActive = allAgents.Count(a => a.State);
            var countInactive = countAll - countActive;
            var countPending = pendingAgents!.Count;

            IEnumerable<AdminConsultAgentDto> source = filter switch
            {
                AgentListFilter.Activos => allAgents.Where(a => a.State),
                AgentListFilter.Inactivos => allAgents.Where(a => !a.State),
                AgentListFilter.Pendientes => pendingAgents,
                _ => allAgents
            };

            var ordered = source
                .OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
                .ThenBy(a => a.LastName, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var totalForFilter = ordered.Count;
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalForFilter / (double)PageSizeValue));
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var pageItems = ordered
                .Skip((page - 1) * PageSizeValue)
                .Take(PageSizeValue)
                .ToList();

            var viewModel = new AgentManagementIndexViewModel
            {
                Agents = _mapper.Map<List<AgentListItemViewModel>>(pageItems),
                Filter = filter,
                TotalForFilter = totalForFilter,
                CountAll = countAll,
                CountActive = countActive,
                CountInactive = countInactive,
                CountPending = countPending,
                Page = page,
                PageSize = PageSizeValue,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ToggleStatusConfirm(string id)
        {
            var agent = await FindAgentAsync(id);
            if (agent == null)
            {
                TempData["ErrorMessage"] = "El agente seleccionado no existe.";
                return RedirectToAction(nameof(Index));
            }

            return View(BuildConfirmViewModel(agent));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id, bool active)
        {
            var dto = new AlterStateUserDto
            {
                Id = id,
                State = active
            };

            var result = await _agentManagementService.ToggleStatusAsync(dto);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description
                    ?? "Ocurrió un error al cambiar el estado del agente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = active
                ? "El agente fue activado correctamente."
                : "El agente fue inactivado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var agent = await FindAgentAsync(id);
            if (agent == null)
            {
                TempData["ErrorMessage"] = "El agente seleccionado no existe.";
                return RedirectToAction(nameof(Index));
            }

            return View(BuildConfirmViewModel(agent));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _agentManagementService.DeleteAgentAsync(id);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description
                    ?? "No fue posible eliminar el agente. Intente nuevamente más tarde.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "El agente fue eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<AdminConsultAgentDto?> FindAgentAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var result = await _agentManagementService.GetAgentsAsync();
            if (!result.IsValid) return null;

            return result.Value!.FirstOrDefault(a => a.Id == id);
        }

        private static AgentActionConfirmViewModel BuildConfirmViewModel(AdminConsultAgentDto agent) => new()
        {
            Id = agent.Id,
            FullName = $"{agent.Name} {agent.LastName}".Trim(),
            IsActive = agent.State,
            Properties = agent.Properties
        };
    }
}
