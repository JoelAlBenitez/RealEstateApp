namespace RealEstateApp.Core.Application.DTOs.Property
{
    public sealed class PropertyImageDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public required string Url { get; set; }
    }
}
