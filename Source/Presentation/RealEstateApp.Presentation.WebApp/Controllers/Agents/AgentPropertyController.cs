using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
// using RealEstateApp.Core.Application.Contracts.PropertyType;
// using RealEstateApp.Core.Application.Contracts.SaleType;
// using RealEstateApp.Core.Application.Contracts.Improvement;
using RealEstateApp.Core.Application.DTOs.Property;
// using RealEstateApp.Core.Application.DTOs.PropertyType;
// using RealEstateApp.Core.Application.DTOs.SaleType;
// using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using AutoMapper;

namespace RealEstateApp.Presentation.WebApp.Controllers.Agents
{
    [Authorize(Roles = "Agente")]
    public class AgentPropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        // private readonly IPropertyTypeService _propertyTypeService;
        // private readonly ISaleTypeService _saleTypeService;
        // private readonly IImprovementService _improvementService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AgentPropertyController(
            IPropertyService propertyService,
            // IPropertyTypeService propertyTypeService,
            // ISaleTypeService saleTypeService,
            // IImprovementService improvementService,
            IUserSession userSession,
            IMapper mapper)
        {
            _propertyService = propertyService;
            // _propertyTypeService = propertyTypeService;
            // _saleTypeService = saleTypeService;
            // _improvementService = improvementService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var agentId = _userSession.GetIdCurrentUser();
            var result = await _propertyService.GetPropertiesByAgentAsync(agentId, 1, int.MaxValue);
            if (!result.IsValid)
            {
                return View(new List<PropertyCardViewModel>());
            }

            var viewModels = _mapper.Map<List<PropertyCardViewModel>>(result.Value);
            // Mostrar solo las disponibles para el CRUD principal de mantenimiento
            var availableProperties = viewModels.Where(p => p.Status == Core.Domain.Common.Enums.PropertyStatus.PropertyState.Available).ToList();
            return View(availableProperties);
        }

        public IActionResult Create()
        {
            var viewModel = new SavePropertyViewModel { Description = string.Empty };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // await PopulateDropdownsAsync(model);
                return View(model);
            }

            var agentId = _userSession.GetIdCurrentUser();
            var dto = _mapper.Map<SavePropertyDto>(model);
            dto.AgentId = agentId;

            var result = await _propertyService.AddAsync(dto);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al crear la propiedad.";
                // await PopulateDropdownsAsync(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var agentId = _userSession.GetIdCurrentUser();
            var result = await _propertyService.GetByIdAsync(id);
            if (!result.IsValid || result.Value == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var property = result.Value;
            if (property.AgentId != agentId)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<SavePropertyViewModel>(property);
            // await PopulateDropdownsAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SavePropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // await PopulateDropdownsAsync(model);
                return View(model);
            }

            var agentId = _userSession.GetIdCurrentUser();
            var currentResult = await _propertyService.GetByIdWithDetailsAsync(model.Id);
            if (!currentResult.IsValid || currentResult.Value == null || currentResult.Value.AgentId != agentId)
            {
                return RedirectToAction(nameof(Index));
            }

            var dto = _mapper.Map<SavePropertyDto>(model);
            dto.AgentId = agentId;

            var result = await _propertyService.UpdateAsync(dto);
            if (result != null && !result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al actualizar la propiedad.";
                // await PopulateDropdownsAsync(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var agentId = _userSession.GetIdCurrentUser();
            var result = await _propertyService.GetByIdWithDetailsAsync(id);
            if (!result.IsValid || result.Value == null || result.Value.AgentId != agentId)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<PropertyCardViewModel>(result.Value);
            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var agentId = _userSession.GetIdCurrentUser();
            var result = await _propertyService.GetByIdWithDetailsAsync(id);
            if (!result.IsValid || result.Value == null || result.Value.AgentId != agentId)
            {
                return RedirectToAction(nameof(Index));
            }

            var deleteResult = await _propertyService.RemoveAsync(id);
            if (!deleteResult.IsValid)
            {
                TempData["ErrorMessage"] = deleteResult.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al eliminar la propiedad.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(SavePropertyViewModel model)
        {
            // var typesResult = await _propertyTypeService.GetAllWithCountAsync();
            // var salesResult = await _saleTypeService.GetAllWithCountAsync();
            // var improvementsResult = await _improvementService.GetAllWithCountAsync();

            // ViewBag.PropertyTypes = typesResult.IsValid && typesResult.Value != null ? typesResult.Value.ToList() : new List<PropertyTypeDto>();
            // ViewBag.SaleTypes = salesResult.IsValid && salesResult.Value != null ? salesResult.Value.ToList() : new List<SaleTypeDto>();
            // ViewBag.Improvements = improvementsResult.IsValid && improvementsResult.Value != null ? improvementsResult.Value.ToList() : new List<ImprovementDto>();
            await Task.CompletedTask;
        }
    }
}
