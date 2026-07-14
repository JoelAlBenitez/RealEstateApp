using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Application.ViewsModel.Offer;
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

        public async Task<IActionResult> Index()
        {
            var result = await _offerService.GetByCustomerAsync();
            if (!result.IsValid)
            {
                return View(new List<OfferViewModel>());
            }

            var viewModels = _mapper.Map<List<OfferViewModel>>(result.Value);

            return View(viewModels);
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

            var dto = new SaveOfferDto
            {
                PropertyId = model.PropertyId,
                Amount = model.Amount,
                CustomerId = string.Empty
            };

            var result = await _offerService.AddAsync(dto);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al crear la oferta.";
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
                TempData["ErrorMessage"] = "Ocurrió un error al cancelar la oferta.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
