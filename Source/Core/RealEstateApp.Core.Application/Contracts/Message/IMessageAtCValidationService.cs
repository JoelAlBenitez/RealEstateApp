using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Message
{
    public interface IMessageAtCValidationService
    {
        Task<ValidationResult> ValidateForCreateAsync(SaveMessageAtCDto dto);
    }
}
