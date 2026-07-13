namespace RealEstateApp.Core.Application.DTOs.PropertyType
{
    public sealed record SavePropertyTypeDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
