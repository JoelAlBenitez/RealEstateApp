using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class SaleType : BaseEntitie<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        
        // Relación inversa con Propiedades
        public ICollection<Property>? Properties { get; set; }
    }
}
