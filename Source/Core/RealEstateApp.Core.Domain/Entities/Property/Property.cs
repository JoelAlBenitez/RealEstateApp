using RealEstateApp.Core.Domain.Common.CodeErrors.Offer;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Domain.Entities.Base;
using System.Diagnostics.CodeAnalysis;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Property : BaseEntitie<int>
    {
        public required string Code { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required double Size { get; set; }
        public required int Bedrooms { get; set; }
        public required int Bathrooms { get; set; }
        public required string AgentId { get; set; }
        public required PropertyState Status { get; set; }
        public required int PropertyTypeId { get; set; }
        public required int SaleTypeId { get; set; }

        // Navigation Properties
       // public PropertyType? PropertyType { get; set; } = null!;
       // public SaleType? SaleType { get; set; } = null!;
        public IReadOnlyCollection<PropertyImage> Images { get; set; } = null!;
        public IReadOnlyCollection<PropertyImprovement> PropertyImprovements { get; set; } = null!;
        public IReadOnlyCollection<Offer> Offers { get; set; } = null!;
    }
}
