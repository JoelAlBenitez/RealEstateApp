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

        public async Task<IActionResult> Index(string? customerId, int? propertyId)
        {
            var result = await _messageService.GetChatsByAgentAsync();
            var viewModels = result.IsValid
                ? _mapper.Map<List<MessageAtCViewModel>>(result.Value)
                : new List<MessageAtCViewModel>();

            if (!string.IsNullOrEmpty(customerId) && propertyId.HasValue)
            {
                var agentId = _userSession.GetIdCurrentUser();
                var convResult = await _messageService.GetChatHistoryAsync(customerId, agentId, propertyId.Value);
                if (convResult.IsValid)
                {
                    ViewBag.ConversationMessages = _mapper.Map<List<MessageAtCViewModel>>(convResult.Value);
                    ViewBag.SelectedCustomerId = customerId;
                    ViewBag.SelectedPropertyId = propertyId.Value;
                }
            }

            return View(viewModels);
        }

        public IActionResult Conversation(string customerId, int propertyId)
        {
            return RedirectToAction(nameof(Index), new { customerId, propertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageAtCViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                TempData["ErrorMessage"] = firstError ?? "El mensaje no es válido.";
                return RedirectToAction(nameof(Index), new { customerId = model.CustomerId, propertyId = model.PropertyId });
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
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al enviar el mensaje.";
            }

            return RedirectToAction(nameof(Index), new { customerId = model.CustomerId, propertyId = model.PropertyId });
        }
    }
}
