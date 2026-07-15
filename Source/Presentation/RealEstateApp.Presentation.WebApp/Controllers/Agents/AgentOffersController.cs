using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.ViewsModel.Offer;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using AutoMapper;

namespace RealEstateApp.Presentation.WebApp.Controllers.Agents
{
    [Authorize(Roles = "Agente")]
    public class AgentOffersController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IPropertyService _propertyService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AgentOffersController(
            IOfferService offerService,
            IPropertyService propertyService,
            IUserSession userSession,
            IMapper mapper)
        {
            _offerService = offerService;
            _propertyService = propertyService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int propertyId)
        {
            var agentId = _userSession.GetIdCurrentUser();
            var propertyResult = await _propertyService.GetByIdWithDetailsAsync(propertyId);
            if (!propertyResult.IsValid || propertyResult.Value == null || propertyResult.Value.AgentId != agentId)
            {
                return RedirectToAction("Index", "Agent");
            }

            var offersResult = await _offerService.GetPendingByPropertyAsync(propertyId);
            if (!offersResult.IsValid)
            {
                return View(new List<OfferViewModel>());
            }

            var viewModels = _mapper.Map<List<OfferViewModel>>(offersResult.Value);
            ViewBag.PropertyId = propertyId;
            ViewBag.PropertyCode = propertyResult.Value.Code;

            return View(viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int offerId, int propertyId)
        {
            var agentId = _userSession.GetIdCurrentUser();
            var propertyResult = await _propertyService.GetByIdWithDetailsAsync(propertyId);
            if (!propertyResult.IsValid || propertyResult.Value == null || propertyResult.Value.AgentId != agentId)
            {
                return RedirectToAction("Index", "Agent");
            }

            var result = await _offerService.AcceptOfferAsync(offerId);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al procesar la aceptación de la oferta.";
            }

            return RedirectToAction(nameof(Index), new { propertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int offerId, int propertyId)
        {
            var agentId = _userSession.GetIdCurrentUser();
            var propertyResult = await _propertyService.GetByIdWithDetailsAsync(propertyId);
            if (!propertyResult.IsValid || propertyResult.Value == null || propertyResult.Value.AgentId != agentId)
            {
                return RedirectToAction("Index", "Agent");
            }

            var result = await _offerService.RejectOfferAsync(offerId);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al procesar el rechazo de la oferta.";
            }

            return RedirectToAction(nameof(Index), new { propertyId });
        }
    }
}
