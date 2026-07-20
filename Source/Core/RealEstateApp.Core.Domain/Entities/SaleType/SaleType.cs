using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class SaleType : BaseEntitie<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
