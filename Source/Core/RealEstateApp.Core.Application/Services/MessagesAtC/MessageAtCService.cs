using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Messages;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Errors;
using Microsoft.Extensions.Logging;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using System.Collections.Generic;
using System.Linq;

namespace RealEstateApp.Core.Application.Services.MessagesAtC
{
    public sealed class MessageAtCService : GenericServices<SaveMessageAtCDto, Domain.Entities.Message, int>, IMessageAtCService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMessageAtCValidationService _validationService;
        private readonly IUserSession _userSession;
        private readonly ILogger<MessageAtCService> _logger;
        private readonly IOperationalAccountWebApp _operationalAccount;

        public MessageAtCService(
            IMessageRepository messageRepository,
            IPropertyRepository propertyRepository,
            IMessageAtCValidationService validationService,
            IMapper mapper,
            IUserSession userSession,
            ILogger<MessageAtCService> logger,
            IOperationalAccountWebApp operationalAccount)
            : base(messageRepository, mapper)
        {
            _messageRepository = messageRepository;
            _propertyRepository = propertyRepository;
            _validationService = validationService;
            _userSession = userSession;
            _logger = logger;
            _operationalAccount = operationalAccount;
        }

        public override async Task<ValidationResult> AddAsync(SaveMessageAtCDto dto)
        {
            try
            {
                var currentUserId = _userSession.GetIdCurrentUser();
                var roles = _userSession.GetRolesCurrentUser();

                if (roles.Contains("Cliente"))
                {
                    dto.CustomerId = currentUserId;
                    dto.IsFromAgent = false;

                    var property = await _propertyRepository.GetByIdAsync(dto.PropertyId);
                    if (property == null)
                    {
                        return ValidationResult.Failure(new Error("Property.NotFound", "La propiedad asociada a la conversación no existe."));
                    }
                    dto.AgentId = property.AgentId;
                }
                else if (roles.Contains("Agente"))
                {
                    dto.AgentId = currentUserId;
                    dto.IsFromAgent = true;
                }

                var validation = await _validationService.ValidateForCreateAsync(dto);
                if (!validation.IsValid)
                {
                    return validation;
                }

                var message = _mapper.Map<Domain.Entities.Message>(dto);
                message.SentAt = DateTime.UtcNow;
                message.CreateAt = DateTimeOffset.UtcNow;
                message.UpdateAt = DateTimeOffset.UtcNow;

                await _messageRepository.AddAsync(message);
                var result = await _messageRepository.SaveAsync();
                if (result > 0)
                {
                    return ValidationResult.Success();
                }
                return ValidationResult.Failure(new Error("Oops", "Ocurrió un error al procesar la solicitud. Favor inténtelo de nuevo más tarde."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en MessageAtCService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            try
            {
                return await base.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en MessageAtCService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatHistoryAsync(string customerId, string agentId, int propertyId)
        {
            try
            {
                var messages = await _messageRepository.GetConversationAsync(customerId, agentId, propertyId);
                var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages).ToList();
                await FillMessageDetailsAsync(dtos);
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en MessageAtCService");
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByAgentAsync()
        {
            try
            {
                var agentId = _userSession.GetIdCurrentUser();
                var messages = await _messageRepository.GetMessagesByAgentAsync(agentId);
                var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages).ToList();
                await FillMessageDetailsAsync(dtos);
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en MessageAtCService");
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByCustomerAsync()
        {
            try
            {
                var customerId = _userSession.GetIdCurrentUser();
                var messages = await _messageRepository.GetMessagesByCustomerAsync(customerId);
                var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages).ToList();
                await FillMessageDetailsAsync(dtos);
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en MessageAtCService");
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        private async Task FillMessageDetailsAsync(List<MessageAtCDto> dtos)
        {
            if (dtos == null || dtos.Count == 0) return;

            var customerIds = dtos.Select(d => d.CustomerId).Distinct().ToList();
            var agentIds = dtos.Select(d => d.AgentId).Distinct().ToList();
            var propertyIds = dtos.Select(d => d.PropertyId).Distinct().ToList();

            var clients = await _operationalAccount.GetClientAllAsync(customerIds);
            var agents = await _operationalAccount.GetAgentAllAsync(agentIds);

            var clientsDict = clients.ToDictionary(c => c.Id, c => $"{c.Name} {c.LastName}");
            var agentsDict = agents.ToDictionary(a => a.Id, a => $"{a.Name} {a.LastName}");

            var properties = new Dictionary<int, string>();
            foreach (var propertyId in propertyIds)
            {
                var property = await _propertyRepository.GetByIdAsync(propertyId);
                if (property != null)
                {
                    properties[propertyId] = property.Code;
                }
            }

            foreach (var dto in dtos)
            {
                if (clientsDict.TryGetValue(dto.CustomerId, out var customerName))
                {
                    dto.CustomerName = customerName;
                }
                else
                {
                    dto.CustomerName = "Usuario desconocido";
                }

                if (agentsDict.TryGetValue(dto.AgentId, out var agentName))
                {
                    dto.AgentName = agentName;
                }
                else
                {
                    dto.AgentName = "Agente desconocido";
                }

                if (properties.TryGetValue(dto.PropertyId, out var code))
                {
                    dto.PropertyCode = code;
                }
                else
                {
                    dto.PropertyCode = "S/C";
                }
            }
        }
    }
}
