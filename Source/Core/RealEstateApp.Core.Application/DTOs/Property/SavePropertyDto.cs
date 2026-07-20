using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;

namespace RealEstateApp.Core.Application.DTOs.Property
{
    public sealed class SavePropertyDto
    {
        public int Id { get; set; }
        public string? Code { get; set; } 
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public double Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string? AgentId { get; set; } 
        public PropertyState Status { get; set; } = PropertyState.Available;
        public int PropertyTypeId { get; set; }
        public int SaleTypeId { get; set; }
        public List<int> ImprovementIds { get; set; } = new();
        public List<IFormFile>? ImageFiles { get; set; }
        public List<string>? ExistingImageUrls { get; set; }
    }
}
