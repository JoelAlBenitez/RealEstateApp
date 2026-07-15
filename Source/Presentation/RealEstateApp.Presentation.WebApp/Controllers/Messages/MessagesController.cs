using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Messages;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Application.ViewsModel.MessageAtC;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using AutoMapper;

namespace RealEstateApp.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class MessagesController : Controller
    {
        private readonly IMessageAtCService _messageService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public MessagesController(IMessageAtCService messageService, IUserSession userSession, IMapper mapper)
        {
            _messageService = messageService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _messageService.GetChatsByCustomerAsync();
            if (!result.IsValid)
            {
                return View(new List<MessageAtCViewModel>());
            }

            var viewModels = _mapper.Map<List<MessageAtCViewModel>>(result.Value);

            return View(viewModels);
        }

        public async Task<IActionResult> Conversation(string agentId, int propertyId)
        {
            var customerId = _userSession.GetIdCurrentUser();
            var result = await _messageService.GetChatHistoryAsync(customerId, agentId, propertyId);
            if (!result.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModels = _mapper.Map<List<MessageAtCViewModel>>(result.Value);

            ViewBag.AgentId = agentId;
            ViewBag.PropertyId = propertyId;

            return View(viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageAtCViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                TempData["ErrorMessage"] = firstError ?? "El mensaje no es válido.";
                return RedirectToAction(nameof(Conversation), new { agentId = model.AgentId, propertyId = model.PropertyId });
            }

            var dto = new SaveMessageAtCDto
            {
                PropertyId = model.PropertyId,
                AgentId = model.AgentId ?? string.Empty,
                Content = model.Content,
                CustomerId = string.Empty
            };

            var result = await _messageService.AddAsync(dto);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al enviar el mensaje.";
            }

            return RedirectToAction(nameof(Conversation), new { agentId = model.AgentId, propertyId = model.PropertyId });
        }
    }
}
