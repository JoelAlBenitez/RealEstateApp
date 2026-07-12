using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;

namespace RealEstateApp.Core.Application.DTOs.Offer
{
    public sealed class SaveOfferDto
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public int PropertyId { get; set; }
        public decimal Amount { get; set; }
        public OfferState Status { get; set; } = OfferState.Pending;
    }
}
