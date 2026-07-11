using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Application.ViewsModel.Offer;

namespace RealEstateApp.Presentation.WebApp.Controllers.Offers
{
    [Authorize(Roles = "Cliente")]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _offerService.GetByCustomerAsync();
            if (!result.IsValid)
            {
                return View(new List<OfferViewModel>());
            }

            var viewModels = result.Value!.Select(o => new OfferViewModel
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                PropertyId = o.PropertyId,
                Amount = o.Amount,
                Status = o.Status,
                CreateAt = o.CreateAt,
                CustomerName = o.CustomerName,
                CustomerEmail = o.CustomerEmail,
                PropertyCode = o.Property != null ? o.Property.Code : null
            }).ToList();

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
