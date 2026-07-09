namespace RealEstateApp.Core.Application.DTOs.MessageAtC
{
    public sealed class SaveMessageAtCDto
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public string? AgentId { get; set; }
        public int PropertyId { get; set; }
        public required string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
