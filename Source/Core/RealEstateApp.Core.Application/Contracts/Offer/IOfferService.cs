using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Offer
{
    public interface IOfferService : IGenericServices<SaveOfferDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<OfferDto>>> GetPendingByPropertyAsync(int propertyId);
        Task<ValidationResult<IReadOnlyCollection<OfferDto>>> GetByCustomerAsync();
        Task<ValidationResult> AcceptOfferAsync(int offerId);
        Task<ValidationResult> RejectOfferAsync(int offerId);
        Task<ValidationResult> CancelOfferAsync(int offerId);
    }
}
