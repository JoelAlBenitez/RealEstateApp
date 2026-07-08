
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Offer
    {
        public required string CustomerId { get; set; }
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public decimal Amount { get; set; }
        public OfferState Status { get; set;}
        }
}
