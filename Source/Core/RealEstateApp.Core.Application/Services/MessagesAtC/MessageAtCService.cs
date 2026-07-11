using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Messages;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Application.Services.MessagesAtC
{
    public sealed class MessageAtCService : GenericServices<SaveMessageAtCDto, Domain.Entities.Message, int>, IMessageAtCService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageAtCValidationService _validationService;
        private readonly IUserSession _userSession;

        public MessageAtCService(
            IMessageRepository messageRepository,
            IMessageAtCValidationService validationService,
            IMapper mapper,
            IUserSession userSession)
            : base(messageRepository, mapper)
        {
            _messageRepository = messageRepository;
            _validationService = validationService;
            _userSession = userSession;
        }

        public override async Task<ValidationResult> AddAsync(SaveMessageAtCDto dto)
        {
            try
            {
                dto.CustomerId = _userSession.GetIdCurrentUser();
                var validation = await _validationService.ValidateForCreateAsync(dto);
                if (!validation.IsValid)
                {
                    return validation;
                }
                return await base.AddAsync(dto);
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            try
            {
                return await base.RemoveAsync(id);
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatHistoryAsync(string customerId, string agentId, int propertyId)
        {
            try
            {
                var messages = await _messageRepository.GetConversationAsync(customerId, agentId, propertyId);
                var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages);
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByAgentAsync()
        {
            try
            {
                var agentId = _userSession.GetIdCurrentUser();
                var messages = await _messageRepository.GetMessagesByAgentAsync(agentId);
                var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages);
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByCustomerAsync()
        {
            try
            {
                var customerId = _userSession.GetIdCurrentUser();
                var messages = await _messageRepository.GetMessagesByCustomerAsync(customerId);
                var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages);
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }
    }
}
