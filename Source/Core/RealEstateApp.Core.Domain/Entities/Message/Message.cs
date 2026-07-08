using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Message : BaseEntitie<int>
    {
        public required string CustomerId { get; set; }
        public required string AgentId { get; set; }
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public required string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
