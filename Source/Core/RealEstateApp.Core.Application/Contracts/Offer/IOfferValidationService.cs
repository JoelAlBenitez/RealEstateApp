using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Offer
{
    public interface IOfferValidationService
    {
        Task<ValidationResult> ValidateForCreateAsync(SaveOfferDto dto);
        Task<ValidationResult> ValidateForAcceptAsync(int offerId, string agentId);
        Task<ValidationResult> ValidateForRejectAsync(int offerId, string agentId);
        Task<ValidationResult> ValidateForCancelAsync(int offerId, string customerId);
    }
}
