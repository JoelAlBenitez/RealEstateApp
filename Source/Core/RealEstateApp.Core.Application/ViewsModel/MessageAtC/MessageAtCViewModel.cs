namespace RealEstateApp.Core.Application.ViewsModel.MessageAtC
{
    public class MessageAtCViewModel
    {
        public int Id { get; set; }
        public required string CustomerId { get; set; }
        public required string AgentId { get; set; }
        public int PropertyId { get; set; }
        public required string Content { get; set; }
        public DateTime SentAt { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public string? CustomerName { get; set; }
        public string? AgentName { get; set; }
    }
}
