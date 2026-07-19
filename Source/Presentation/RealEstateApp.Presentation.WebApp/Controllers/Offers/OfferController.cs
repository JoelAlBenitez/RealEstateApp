using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Application.ViewsModel.Offer;
using RealEstateApp.Core.Application.ViewsModel.Property;
using AutoMapper;

namespace RealEstateApp.Presentation.WebApp.Controllers.Offers
{
    [Authorize(Roles = "Cliente")]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;

        public OfferController(IOfferService offerService, IMapper mapper)
        {
            _offerService = offerService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            if (pageNumber < 1) pageNumber = 1;
            var result = await _offerService.GetByCustomerAsync();
            if (!result.IsValid)
            {
                return View(new CustomerOffersViewModel { Offers = new List<OfferViewModel>() });
            }

            var viewModels = _mapper.Map<List<OfferViewModel>>(result.Value);
            var totalItems = viewModels.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            var paginatedViewModels = viewModels.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new CustomerOffersViewModel
            {
                Offers = paginatedViewModels,
                Page = pageNumber,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOfferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                TempData["ErrorMessage"] = firstError ?? "El monto ingresado no es válido.";
                return RedirectToAction("Details", "Customer", new { id = model.PropertyId });
            }

            var dto = _mapper.Map<SaveOfferDto>(model);

            var result = await _offerService.AddAsync(dto);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al crear la oferta.";
                return RedirectToAction("Details", "Customer", new { id = model.PropertyId });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int offerId)
        {
            var result = await _offerService.CancelOfferAsync(offerId);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al cancelar la oferta.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
