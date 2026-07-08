using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class PropertyImage : BaseEntitie<int>
    {
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public required string ImagePath { get; set; }
    }
}
