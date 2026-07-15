using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Application.ViewsModel.FavoriteProperty;
using RealEstateApp.Core.Application.ViewsModel.Property;
using AutoMapper;

namespace RealEstateApp.Presentation.WebApp.Controllers.FavoriteProperties
{
    [Authorize(Roles = "Cliente")]
    public class FavoriteController : Controller
    {
        private readonly IFavoritePropertyService _favoritePropertyService;
        private readonly IMapper _mapper;

        public FavoriteController(IFavoritePropertyService favoritePropertyService, IMapper mapper)
        {
            _favoritePropertyService = favoritePropertyService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _favoritePropertyService.GetByCustomerAsync();
            if (!result.IsValid)
            {
                return View(new List<FavoritePropertyViewModel>());
            }

            var viewModels = _mapper.Map<List<FavoritePropertyViewModel>>(result.Value);
            viewModels.ForEach(vm =>
            {
                if (vm.Property != null)
                {
                    vm.Property.IsFavorite = true;
                }
            });

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
