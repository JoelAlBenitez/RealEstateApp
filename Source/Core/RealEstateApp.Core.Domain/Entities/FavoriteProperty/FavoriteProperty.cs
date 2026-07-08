using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class FavoriteProperty : BaseEntitie<int>
    {
        public required string CustomerId { get; set; }
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
    }
}
