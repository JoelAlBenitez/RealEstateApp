using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Messages;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Application.ViewsModel.MessageAtC;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using AutoMapper;

namespace RealEstateApp.Presentation.WebApp.Controllers.Agents
{
    [Authorize(Roles = "Agente")]
    public class AgentConversationsController : Controller
    {
        private readonly IMessageAtCService _messageService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AgentConversationsController(
            IMessageAtCService messageService,
            IUserSession userSession,
            IMapper mapper)
        {
            _messageService = messageService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _messageService.GetChatsByAgentAsync();
            if (!result.IsValid)
            {
                return View(new List<MessageAtCViewModel>());
            }

            var viewModels = _mapper.Map<List<MessageAtCViewModel>>(result.Value);
            return View(viewModels);
        }

        public async Task<IActionResult> Conversation(string customerId, int propertyId)
        {
            var agentId = _userSession.GetIdCurrentUser();
            var result = await _messageService.GetChatHistoryAsync(customerId, agentId, propertyId);
            if (!result.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModels = _mapper.Map<List<MessageAtCViewModel>>(result.Value);
            ViewBag.CustomerId = customerId;
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
                return RedirectToAction(nameof(Conversation), new { customerId = model.CustomerId, propertyId = model.PropertyId });
            }

            var dto = new SaveMessageAtCDto
            {
                PropertyId = model.PropertyId,
                CustomerId = model.CustomerId ?? string.Empty,
                Content = model.Content,
                AgentId = string.Empty
            };

            var result = await _messageService.AddAsync(dto);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al enviar el mensaje.";
            }

            return RedirectToAction(nameof(Conversation), new { customerId = model.CustomerId, propertyId = model.PropertyId });
        }
    }
}
