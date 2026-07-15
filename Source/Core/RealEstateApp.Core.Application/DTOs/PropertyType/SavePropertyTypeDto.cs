namespace RealEstateApp.Core.Application.DTOs.PropertyType
{
    public sealed record SavePropertyTypeDto
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
