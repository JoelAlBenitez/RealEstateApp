namespace RealEstateApp.Core.Application.DTOs.Message
{
    public class MessageDto
    {
        public required string To { get; set; }
        public  required string Body { get; set; }
        public required string Subject { get; set; }
    }
}
