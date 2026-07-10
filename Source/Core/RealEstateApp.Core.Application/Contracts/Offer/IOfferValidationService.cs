using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Offer
{
    public interface IOfferValidationService
    {
        Task<ValidationResult> ValidateForCreateAsync(SaveOfferDto dto);
        Task<ValidationResult> ValidateForAcceptAsync(int offerId);
        Task<ValidationResult> ValidateForRejectAsync(int offerId);
        Task<ValidationResult> ValidateForCancelAsync(int offerId);
    }
}
