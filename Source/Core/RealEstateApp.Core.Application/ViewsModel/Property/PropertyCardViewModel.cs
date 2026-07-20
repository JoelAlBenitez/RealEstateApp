using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;

namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public class PropertyCardViewModel
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
        public string? ImageUrl { get; set; } 
        public bool IsFavorite { get; set; }
        public string? PropertyTypeName { get; set; }
        public string? SaleTypeName { get; set; }
    }
}
