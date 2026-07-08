
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
using RealEstateApp.Core.Domain.Entities.Base;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Offer : BaseEntitie<int>
    {
        public required string CustomerId { get; set; }
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public decimal Amount { get; set; }
        public OfferState Status { get; set;}
        }
}
