using RealEstateApp.Core.Domain.Entities.Base;
using RealEstateApp.Core.Domain.Enums;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Property : BaseEntitie<int>
    {
        public required string Code { get; set; } // Código único de 6 dígitos
        public decimal Price { get; set; } // En pesos dominicanos DOP
        public required string Description { get; set; }
        public double Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public required string AgentId { get; set; } // Propietario del inmueble
        public PropertyStatus Status { get; set; } // Enum (Disponible, Vendida)

        // Relaciones
        public int PropertyTypeId { get; set; }
        public PropertyType? PropertyType { get; set; }

        public int SaleTypeId { get; set; }
        public SaleType? SaleType { get; set; }

        public ICollection<PropertyImage>? Images { get; set; }
        public ICollection<PropertyImprovement>? PropertyImprovements { get; set; }
        public ICollection<Offer>? Offers { get; set; }
    }
}
