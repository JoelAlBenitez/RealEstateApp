using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infraestructure.Persistence.Repositories
{
    public sealed class MessageRepository : GenericRepository<Message, int>, IMessageRepository
    {
        public MessageRepository(DbContextRealEstateApp context) : base(context) { }

        public async Task<IReadOnlyCollection<Message>> GetConversationAsync(string customerId, string agentId, int propertyId)
        {
            return await _context.Messages
                .Where(m => m.CustomerId == customerId && m.AgentId == agentId && m.PropertyId == propertyId)
                .OrderBy(m => m.SentAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Message>> GetMessagesByAgentAsync(string agentId)
        {
            return await _context.Messages
                .Where(m => m.AgentId == agentId)
                .OrderByDescending(m => m.SentAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Message>> GetMessagesByCustomerAsync(string customerId)
        {
            return await _context.Messages
                .Where(m => m.CustomerId == customerId)
                .OrderByDescending(m => m.SentAt)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
