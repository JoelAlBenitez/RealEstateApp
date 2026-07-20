using RealEstateApp.Core.Domain.Entities.Base;
namespace RealEstateApp.Core.Domain.Entities
{
    public class PropertyType : BaseEntitie<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
