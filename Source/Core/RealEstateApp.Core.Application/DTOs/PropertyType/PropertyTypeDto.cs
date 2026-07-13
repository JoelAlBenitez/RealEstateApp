namespace RealEstateApp.Core.Application.DTOs.PropertyType
{
    public sealed record PropertyTypeDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int PropertyCount { get; set; }
    }
}
