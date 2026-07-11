using RealEstateApp.Core.Application.ViewsModel.Offer;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;

namespace RealEstateApp.Core.Application.ViewsModel.Property
{
    public class PropertyDetailViewModel
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
        public string? AgentName { get; set; }
        public string? AgentPhone { get; set; }
        public string? AgentEmail { get; set; }
        public string? AgentPhotoUrl { get; set; }

        public List<string> ImageUrls { get; set; } = null!;
        public bool IsFavorite { get; set; }
        public bool HasPendingOffer { get; set; }

        public List<OfferViewModel> Offers { get; set; } = null!;

        // Catálogos comentados por límites de módulo
        // public string? PropertyTypeName { get; set; }
        // public string? SaleTypeName { get; set; }
        // public List<string> Improvements { get; set; } = new();
    }
}
