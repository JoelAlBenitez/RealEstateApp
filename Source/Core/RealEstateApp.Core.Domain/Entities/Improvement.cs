using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Improvement : BaseEntitie<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        
        // Relación N:N con Propiedades
        public ICollection<PropertyImprovement>? PropertyImprovements { get; set; }
    }
}
