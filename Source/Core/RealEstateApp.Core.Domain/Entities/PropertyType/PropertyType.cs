using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class PropertyType : BaseEntitie<int>
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        // Navigation Properties
        public IReadOnlyCollection<Property> Properties { get; set; } = null!; 
    }
}
