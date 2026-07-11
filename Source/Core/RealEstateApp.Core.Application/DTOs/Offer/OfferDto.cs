using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;

namespace RealEstateApp.Core.Application.DTOs.Offer
{
    public sealed class OfferDto
    {
        public int Id { get; set; }
        public required string CustomerId { get; set; }
        public int PropertyId { get; set; }
        public decimal Amount { get; set; }
        public OfferState Status { get; set; }
        public DateTimeOffset CreateAt { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public PropertyDto? Property { get; set; }
    }
}
