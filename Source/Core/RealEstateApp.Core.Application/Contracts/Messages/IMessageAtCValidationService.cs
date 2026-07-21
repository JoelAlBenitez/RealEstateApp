using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Messages
{
    public interface IMessageAtCValidationService
    {
        Task<ValidationResult> ValidateForCreateAsync(SaveMessageAtCDto dto);
    }
}
