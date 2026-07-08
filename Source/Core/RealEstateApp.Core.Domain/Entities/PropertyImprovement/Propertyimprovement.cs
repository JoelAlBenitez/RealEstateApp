using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Propertyimprovement : BaseEntitie<int>
    {
        public required int PropertyId { get; set; }
        public required int ImprovementId { get; set; }

        // Navigation Properties
        public Property? Property { get; set; }
        public Improvement? Improvement { get; set; }
    }
}
