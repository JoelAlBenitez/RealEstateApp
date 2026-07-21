using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.PropertyType;
using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Application.ViewsModel.Offer;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.ViewsModel.Common;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class CustomerController : Controller
    {
        private readonly IPropertyQueryService _propertyQueryService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IFavoritePropertyService _favoritePropertyService;
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;
        private readonly IOperationalAccountWebApp _accountWebApp;

        public CustomerController(
            IPropertyQueryService propertyQueryService,
            IPropertyTypeService propertyTypeService,
            IFavoritePropertyService favoritePropertyService,
            IOfferService offerService,
            IMapper mapper,
            IOperationalAccountWebApp accountWebApp)
        {
            _propertyQueryService = propertyQueryService;
            _propertyTypeService = propertyTypeService;
            _favoritePropertyService = favoritePropertyService;
            _offerService = offerService;
            _mapper = mapper;
            _accountWebApp = accountWebApp;
        }

        public async Task<IActionResult> Index(PropertyFilterViewModel filter, int pageNumber = 1, int pageSize = 6)
        {
            if (!ModelState.IsValid)
            {
                filter.TypePropery = await GetPropertyTypeOptionsAsync();
                return View(new CustomerPropertiesViewModel
                {
                    Properties = new List<PropertyCardViewModel>(),
                    Filter = filter
                });
            }

            return View(await BuildAvailablePropertiesAsync(filter, pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FilterProperty(PropertyFilterViewModel filter)
        {
            if (!ModelState.IsValid)
            {
                TempData["Warning"] = "La búsqueda no pudo ser realizada. Favor revise los valores ingresados.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), new
            {
                filter.IdTypeProperty,
                filter.MinPrice,
                filter.MaxPrice,
                filter.Bedrooms,
                filter.Bathrooms
            });
        }

        private async Task<CustomerPropertiesViewModel> BuildAvailablePropertiesAsync(
            PropertyFilterViewModel filter, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;

            var filters = _mapper.Map<PropertyFilterDto>(filter);

            var countResult = await _propertyQueryService.GetAvailableCountAsync(filters);
            var totalItems = countResult.IsValid ? countResult.Value : 0;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            filter.TypePropery = await GetPropertyTypeOptionsAsync();

            var result = await _propertyQueryService.GetAvailableAsync(filters, pageNumber, pageSize);
            if (!result.IsValid)
            {
                return new CustomerPropertiesViewModel
                {
                    Properties = new List<PropertyCardViewModel>(),
                    Filter = filter
                };
            }

            var favoriteIds = await GetFavoritePropertyIdsAsync();

            var viewModels = _mapper.Map<List<PropertyCardViewModel>>(result.Value);
            viewModels.ForEach(vm => vm.IsFavorite = favoriteIds.Contains(vm.Id));

            return new CustomerPropertiesViewModel
            {
                Properties = viewModels,
                Filter = filter,
                Page = pageNumber,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize
            };
        }

        private async Task<HashSet<int>> GetFavoritePropertyIdsAsync()
        {
            var favoritesResult = await _favoritePropertyService.GetByCustomerAsync();
            return favoritesResult.IsValid && favoritesResult.Value != null
                ? favoritesResult.Value.Select(f => f.PropertyId).ToHashSet()
                : new HashSet<int>();
        }

        private async Task<List<TypePropertyViewModel>> GetPropertyTypeOptionsAsync()
        {
            var result = await _propertyTypeService.GetAllForSelectAsync();
            if (!result.IsValid || result.Value == null)
            {
                return new List<TypePropertyViewModel>();
            }
            return result.Value
                .Select(t => new TypePropertyViewModel { Id = t.Id, Name = t.Name })
                .ToList();
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _propertyQueryService.GetByIdWithDetailsAsync(id);
            if (!result.IsValid || result.Value == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var p = result.Value;

            var favoritesResult = await _favoritePropertyService.GetByCustomerAsync();
            var isFavorite = favoritesResult.IsValid && favoritesResult.Value != null &&
                             favoritesResult.Value.Any(f => f.PropertyId == id);

            var offersResult = await _offerService.GetByCustomerAsync();
            var hasPendingOffer = offersResult.IsValid && offersResult.Value != null &&
                                  offersResult.Value.Any(o => o.PropertyId == id && o.Status == OfferState.Pending);

            var propertyOffersResult = await _offerService.GetOffersByCustomerAndPropertyAsync(id);
            var propertyOffers = propertyOffersResult.IsValid && propertyOffersResult.Value != null
                ? _mapper.Map<List<OfferViewModel>>(propertyOffersResult.Value)
                : new List<OfferViewModel>();

            var viewModel = _mapper.Map<PropertyDetailViewModel>(p);
            viewModel.IsFavorite = isFavorite;
            viewModel.HasPendingOffer = hasPendingOffer;
            viewModel.Offers = propertyOffers;

            return View(viewModel);
        }

        public async Task<IActionResult> Agents()
        {
            var result = await _accountWebApp.GetAgentAllViewHomeByCustomer();
            var agents = result == null
                ? Array.Empty<AgentViewModel>()
                : _mapper.Map<IReadOnlyCollection<AgentViewModel>>(result);

            return View(new AgentsHomeViewModel
            {
                Agents = agents ?? Array.Empty<AgentViewModel>(),
                Consult = new AgentConsultByNameOrLastNameViewModel { Name = "" }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgentsConsult(AgentConsultByNameOrLastNameViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Agents));
            }
            var map = _mapper.Map<ConsultAgentByNameOrLastNameDto>(vm);
            var result = await _accountWebApp.GetAgentByConsultCustomer(map);
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

        public async Task<IActionResult> PropertyByAgent(string AgentId, int pageNumber = 1, int pageSize = 6)
        {
            if (pageNumber < 1) pageNumber = 1;

            var agentResult = await _accountWebApp.GetConsultAgentById(AgentId);
            if (agentResult == null)
            {
                TempData["Warning"] = "El agente solicitado no existe o no se encuentra disponible.";
                return RedirectToAction(nameof(Agents));
            }

            var countResult = await _propertyQueryService.CountAvailableByAgentAsync(AgentId);
            var totalItems = countResult.IsValid ? countResult.Value : 0;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            var result = await _propertyQueryService.GetAvailableByAgentAsync(AgentId, pageNumber, pageSize);
            if (!result.IsValid)
            {
                ViewBag.AgentName = $"{agentResult.Name} {agentResult.LastName}";
                ViewBag.AgentId = AgentId;
                return View(new CustomerPropertiesViewModel { Properties = new List<PropertyCardViewModel>() });
            }

            var favoriteIds = await GetFavoritePropertyIdsAsync();

            var viewModels = _mapper.Map<List<PropertyCardViewModel>>(result.Value);
            viewModels.ForEach(vm => vm.IsFavorite = favoriteIds.Contains(vm.Id));

            var viewModel = new CustomerPropertiesViewModel
            {
                Properties = viewModels,
                Page = pageNumber,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize
            };

            ViewBag.AgentName = $"{agentResult.Name} {agentResult.LastName}";
            ViewBag.AgentId = AgentId;

            return View(viewModel);
        }
    }
}
