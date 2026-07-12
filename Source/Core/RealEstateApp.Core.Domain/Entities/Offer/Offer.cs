
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Offer : BaseEntitie<int>
    {
        public required string CustomerId { get; set; }
        public required int PropertyId { get; set; }
        public required decimal Amount { get; set; }
        public required OfferState Status { get; set; }

        // Navigation Properties
        public Property? Property { get; set; }
    }
}
