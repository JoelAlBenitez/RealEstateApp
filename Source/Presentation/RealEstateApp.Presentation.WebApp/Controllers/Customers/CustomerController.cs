using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Application.ViewsModel.Offer;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
using AutoMapper;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class CustomerController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IFavoritePropertyService _favoritePropertyService;
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;

        public CustomerController(
            IPropertyService propertyService,
            IFavoritePropertyService favoritePropertyService,
            IOfferService offerService,
            IMapper mapper)
        {
            _propertyService = propertyService;
            _favoritePropertyService = favoritePropertyService;
            _offerService = offerService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(PropertyFilterDto filters, int pageNumber = 1, int pageSize = 6)
        {
            if (pageNumber < 1) pageNumber = 1;

            var countResult = await _propertyService.GetAvailableCountAsync(filters);
            var totalItems = countResult.IsValid ? countResult.Value : 0;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            var result = await _propertyService.GetAvailableAsync(filters, pageNumber, pageSize);
            if (!result.IsValid)
            {
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 1;
                ViewBag.TotalItems = 0;
                return View(new List<PropertyCardViewModel>());
            }

            var favoritesResult = await _favoritePropertyService.GetByCustomerAsync();
            var favoriteIds = favoritesResult.IsValid && favoritesResult.Value != null
                ? favoritesResult.Value.Select(f => f.PropertyId).ToHashSet()
                : new HashSet<int>();

            var viewModels = _mapper.Map<List<PropertyCardViewModel>>(result.Value);
            viewModels.ForEach(vm => vm.IsFavorite = favoriteIds.Contains(vm.Id));

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            ViewBag.PropertyTypeId = filters.PropertyTypeId;
            ViewBag.MinPrice = filters.MinPrice;
            ViewBag.MaxPrice = filters.MaxPrice;
            ViewBag.Bedrooms = filters.Bedrooms;
            ViewBag.Bathrooms = filters.Bathrooms;

            return View(viewModels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _propertyService.GetByIdWithDetailsAsync(id);
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
    }
}
