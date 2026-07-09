using AutoMapper;
using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.Contracts.Message;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.MessageAtC
{
    public sealed class MessageAtCService : IMessageAtCService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageAtCValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly IGenericServices<SaveMessageAtCDto, int> _genericService;

        public MessageAtCService(
            IMessageRepository messageRepository,
            IMessageAtCValidationService validationService,
            IMapper mapper,
            IGenericServices<SaveMessageAtCDto, int> genericService)
        {
            _messageRepository = messageRepository;
            _validationService = validationService;
            _mapper = mapper;
            _genericService = genericService;
        }

        public async Task<ValidationResult> AddAsync(SaveMessageAtCDto dto)
        {
            var validation = await _validationService.ValidateForCreateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await _genericService.AddAsync(dto);
        }

        public async Task<ValidationResult?> UpdateAsync(SaveMessageAtCDto dto, int id)
        {
            return await _genericService.UpdateAsync(dto, id);
        }

        public async Task<ValidationResult<SaveMessageAtCDto>> GetByIdAsync(int id)
        {
            return await _genericService.GetByIdAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<SaveMessageAtCDto>>> GetAllAsync()
        {
            return await _genericService.GetAllAsync();
        }

        public async Task<ValidationResult> DeleteAsync(int id)
        {
            return await _genericService.DeleteAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatHistoryAsync(string customerId, string agentId, int propertyId)
        {
            var messages = await _messageRepository.GetConversationAsync(customerId, agentId, propertyId);
            var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages);
            return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByAgentAsync(string agentId)
        {
            var messages = await _messageRepository.GetMessagesByAgentAsync(agentId);
            var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages);
            return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
        }

        public async Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByCustomerAsync(string customerId)
        {
            var messages = await _messageRepository.GetMessagesByCustomerAsync(customerId);
            var dtos = _mapper.Map<IReadOnlyCollection<MessageAtCDto>>(messages);
            return ValidationResult<IReadOnlyCollection<MessageAtCDto>>.Success(dtos);
        }

        public async Task<ValidationResult> RemoveAsync(int id)
        {
            return await _genericService.RemoveAsync(id);
        }
    }
}
