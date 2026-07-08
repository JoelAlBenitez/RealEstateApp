using RealEstateApp.Core.Domain.Common.CodeErrors.Offer;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Domain.Entities.Base;
using System.Diagnostics.CodeAnalysis;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Property : BaseEntitie<int>
    {
        public required string Code { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public double Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public required string AgentId { get; set; }
        public PropertyState Status { get; set; }

        //Navigation properties

        public int PropertyTypeId { get; set; }
        public PropertyType? PropertyType { get; set; } = null!;

        public int SaleTypeId { get; set; }
        public SaleType? SaleType { get; set; } = null!;

        public IReadOnlyCollection<PropertyImage> Images { get; set; } = null!;
        public IReadOnlyCollection<Propertyimprovement> PropertyImprovements { get; set; } = null!;
        public IReadOnlyCollection<Offer?> Offers { get; set; } = null!;
    }
}
