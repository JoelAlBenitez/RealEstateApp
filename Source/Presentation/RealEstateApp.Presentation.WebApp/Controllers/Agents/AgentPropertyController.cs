using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.Contracts.Messages;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Application.ViewsModel.Common;
using RealEstateApp.Core.Application.ViewsModel.Offer;
using RealEstateApp.Core.Application.ViewsModel.MessageAtC;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using AutoMapper;

namespace RealEstateApp.Presentation.WebApp.Controllers.Agents
{
    [Authorize(Roles = "Agente")]
    public class AgentPropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IOfferService _offerService;
        private readonly IMessageAtCService _messageService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AgentPropertyController(
            IPropertyService propertyService,
            IOfferService offerService,
            IMessageAtCService messageService,
            IUserSession userSession,
            IMapper mapper)
        {
            _propertyService = propertyService;
            _offerService = offerService;
            _messageService = messageService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;

            var agentId = _userSession.GetIdCurrentUser();
            var countResult = await _propertyService.CountByAgentAsync(agentId);
            var totalItems = countResult.IsValid ? countResult.Value : 0;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            var result = await _propertyService.GetPropertiesByAgentAsync(agentId, pageNumber, pageSize);
            if (!result.IsValid)
            {
                return View(new AgentPropertiesViewModel { Properties = new List<PropertyCardViewModel>() });
            }

            var viewModels = _mapper.Map<List<PropertyCardViewModel>>(result.Value);

            var viewModel = new AgentPropertiesViewModel
            {
                Properties = viewModels,
                Page = pageNumber,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize
            };

            return View(viewModel);
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
            var filesCount = model.ImageFiles?.Count ?? 0;
            if (filesCount < 1 || filesCount > 4)
            {
                ModelState.AddModelError("ImageFiles", "Debe seleccionar entre 1 y 4 imágenes para crear la propiedad.");
            }

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
            var currentCount = (model.ExistingImageUrls?.Count ?? 0) + (model.ImageFiles?.Count ?? 0);
            if (currentCount < 1 || currentCount > 4)
            {
                ModelState.AddModelError("ImageFiles", "El total de imágenes de la propiedad (existentes conservadas + nuevas agregadas) debe ser de al menos 1 y no exceder las 4 unidades.");
            }

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

        public async Task<IActionResult> Details(int id)
        {
            var agentId = _userSession.GetIdCurrentUser();
            var result = await _propertyService.GetByIdWithDetailsAsync(id);
            if (!result.IsValid || result.Value == null || result.Value.AgentId != agentId)
            {
                return RedirectToAction(nameof(Index));
            }

            var propertyVm = _mapper.Map<PropertyDetailViewModel>(result.Value);

            var offersResult = await _offerService.GetOffersByPropertyAsync(id);
            var offers = offersResult.IsValid && offersResult.Value != null
                ? _mapper.Map<List<OfferViewModel>>(offersResult.Value)
                : new List<OfferViewModel>();

            var chatsResult = await _messageService.GetChatsByAgentAsync();
            var conversations = chatsResult.IsValid && chatsResult.Value != null
                ? _mapper.Map<List<MessageAtCViewModel>>(chatsResult.Value.Where(m => m.PropertyId == id))
                : new List<MessageAtCViewModel>();

            var viewModel = new AgentPropertyDetailsViewModel
            {
                Property = propertyVm,
                Offers = offers,
                Conversations = conversations
            };

            return View(viewModel);
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
