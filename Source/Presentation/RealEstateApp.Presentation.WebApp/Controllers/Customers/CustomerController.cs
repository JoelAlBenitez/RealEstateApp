using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Application.ViewsModel.Offer;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class CustomerController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IFavoritePropertyService _favoritePropertyService;
        private readonly IOfferService _offerService;

        public CustomerController(
            IPropertyService propertyService,
            IFavoritePropertyService favoritePropertyService,
            IOfferService offerService)
        {
            _propertyService = propertyService;
            _favoritePropertyService = favoritePropertyService;
            _offerService = offerService;
        }

        public async Task<IActionResult> Index(PropertyFilterDto filters)
        {
            var result = await _propertyService.GetAvailableAsync(filters);
            if (!result.IsValid)
            {
                return View(new List<PropertyCardViewModel>());
            }

            var favoritesResult = await _favoritePropertyService.GetByCustomerAsync();
            var favoriteIds = favoritesResult.IsValid && favoritesResult.Value != null
                ? favoritesResult.Value.Select(f => f.PropertyId).ToHashSet()
                : new HashSet<int>();

            var viewModels = result.Value!.Select(p => new PropertyCardViewModel
            {
                Id = p.Id,
                Code = p.Code,
                Price = p.Price,
                Description = p.Description,
                Size = p.Size,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                AgentId = p.AgentId,
                Status = p.Status,
                ImageUrl = p.Images != null && p.Images.Any() ? p.Images.First().Url : null,
                IsFavorite = favoriteIds.Contains(p.Id)
            }).ToList();

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

            var propertyOffersResult = await _offerService.GetPendingByPropertyAsync(id);
            var propertyOffers = propertyOffersResult.IsValid && propertyOffersResult.Value != null
                ? propertyOffersResult.Value.Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    PropertyId = o.PropertyId,
                    Amount = o.Amount,
                    Status = o.Status,
                    CreateAt = o.CreateAt,
                    CustomerName = o.CustomerName
                }).ToList()
                : new List<OfferViewModel>();

            var viewModel = new PropertyDetailViewModel
            {
                Id = p.Id,
                Code = p.Code,
                Price = p.Price,
                Description = p.Description,
                Size = p.Size,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                AgentId = p.AgentId,
                Status = p.Status,
                AgentName = p.AgentName,
                AgentPhone = p.AgentPhone,
                AgentEmail = p.AgentEmail,
                AgentPhotoUrl = p.AgentPhotoUrl,
                ImageUrls = p.Images != null ? p.Images.Select(i => i.Url).ToList() : new List<string>(),
                IsFavorite = isFavorite,
                HasPendingOffer = hasPendingOffer,
                Offers = propertyOffers
            };

            return View(viewModel);
        }
    }
}
