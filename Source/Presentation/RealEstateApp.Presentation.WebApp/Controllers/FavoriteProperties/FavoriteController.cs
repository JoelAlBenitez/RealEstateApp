using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Application.ViewsModel.FavoriteProperty;
using RealEstateApp.Core.Application.ViewsModel.Property;

namespace RealEstateApp.Presentation.WebApp.Controllers.FavoriteProperties
{
    [Authorize(Roles = "Cliente")]
    public class FavoriteController : Controller
    {
        private readonly IFavoritePropertyService _favoritePropertyService;

        public FavoriteController(IFavoritePropertyService favoritePropertyService)
        {
            _favoritePropertyService = favoritePropertyService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _favoritePropertyService.GetByCustomerAsync();
            if (!result.IsValid)
            {
                return View(new List<FavoritePropertyViewModel>());
            }

            var viewModels = result.Value!.Select(f => new FavoritePropertyViewModel
            {
                Id = f.Id,
                CustomerId = f.CustomerId,
                PropertyId = f.PropertyId,
                Property = f.Property != null ? new PropertyCardViewModel
                {
                    Id = f.Property.Id,
                    Code = f.Property.Code,
                    Price = f.Property.Price,
                    Description = f.Property.Description,
                    Size = f.Property.Size,
                    Bedrooms = f.Property.Bedrooms,
                    Bathrooms = f.Property.Bathrooms,
                    AgentId = f.Property.AgentId,
                    Status = f.Property.Status,
                    ImageUrl = f.Property.Images != null && f.Property.Images.Any() ? f.Property.Images.First().Url : null,
                    IsFavorite = true
                } : null
            }).ToList();

            return View(viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int propertyId)
        {
            var dto = new SaveFavoritePropertyDto
            {
                PropertyId = propertyId,
                CustomerId = string.Empty
            };

            var result = await _favoritePropertyService.AddAsync(dto);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al agregar la propiedad a favoritos.";
            }

            return RedirectToAction("Details", "Customer", new { id = propertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int propertyId)
        {
            var result = await _favoritePropertyService.RemoveFavoriteAsync(propertyId);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al remover la propiedad de favoritos.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
