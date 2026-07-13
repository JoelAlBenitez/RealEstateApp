using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class PropertyImage : BaseEntitie<int>
    {
        public required int PropertyId { get; set; }
        public required string ImageUrl { get; set; }

        // Navigation Properties
        public Property? Property { get; set; }
    }
}
