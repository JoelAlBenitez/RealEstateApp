using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;

namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message, int>
    {
        Task<IReadOnlyCollection<Message>> GetConversationAsync(string customerId, string agentId, int propertyId);
    }
}
