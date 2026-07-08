using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Propertyimprovement : BaseEntitie<int>
    {
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public int ImprovementId { get; set; }
        public Improvement? Improvement { get; set; }
    }
}
