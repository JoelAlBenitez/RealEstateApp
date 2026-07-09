using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Message
{
    public interface IMessageAtCService : IGenericServices<SaveMessageAtCDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatHistoryAsync(string customerId, string agentId, int propertyId);
        Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByAgentAsync(string agentId);
        Task<ValidationResult<IReadOnlyCollection<MessageAtCDto>>> GetChatsByCustomerAsync(string customerId);
    }
}
