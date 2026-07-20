using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;

namespace RealEstateApp.Core.Application.DTOs.Property
{
    public sealed class PropertyDto
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public double Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public required string AgentId { get; set; }
        public PropertyState Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public string? AgentName { get; set; }
        public string? AgentPhone { get; set; }
        public string? AgentEmail { get; set; }
        public string? AgentPhotoUrl { get; set; }
        public int PropertyTypeId { get; set; }
        public string? PropertyTypeName { get; set; }
        public int SaleTypeId { get; set; }
        public string? SaleTypeName { get; set; }

        public List<PropertyImageDto> Images { get; set; } = null!;
        public List<string> Improvements { get; set; } = new();
    }
}
