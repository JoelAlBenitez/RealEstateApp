using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Message : BaseEntitie<int>
    {
        public required string CustomerId { get; set; }
        public required string AgentId { get; set; }
        public required int PropertyId { get; set; }
        public required string Content { get; set; }
        public required DateTime SentAt { get; set; }
        public required bool IsFromAgent { get; set; }

        // Navigation Properties
        public Property? Property { get; set; }
    }
}
