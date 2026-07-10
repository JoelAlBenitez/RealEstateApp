using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Message;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;

namespace RealEstateApp.Core.Application.Services.MessageAtC
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
            dto.CustomerId = _userSession.GetIdCurrentUser();
            var validation = await _validationService.ValidateForCreateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await base.AddAsync(dto);
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            return await base.RemoveAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatHistoryAsync(string customerId, string agentId, int propertyId)
        {
            var messages = await _messageRepository.GetConversationAsync(customerId, agentId, propertyId);
            var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages);
            return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByAgentAsync()
        {
            var agentId = _userSession.GetIdCurrentUser();
            var messages = await _messageRepository.GetMessagesByAgentAsync(agentId);
            var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages);
            return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByCustomerAsync()
        {
            var customerId = _userSession.GetIdCurrentUser();
            var messages = await _messageRepository.GetMessagesByCustomerAsync(customerId);
            var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages);
            return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
        }
    }
}
