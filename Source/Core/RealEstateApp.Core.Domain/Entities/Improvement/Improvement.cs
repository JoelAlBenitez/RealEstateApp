using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Improvement : BaseEntitie<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        //Navigation properties
        public IReadOnlyCollection<Propertyimprovement> PropertyImprovements { get; set; } = null!;
    }
}
