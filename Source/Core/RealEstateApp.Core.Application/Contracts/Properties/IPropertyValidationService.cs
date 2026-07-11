using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Properties
{
    public interface IPropertyValidationService
    {
        Task<ValidationResult> ValidateForCreateAsync(SavePropertyDto dto);
        Task<ValidationResult> ValidateForUpdateAsync(SavePropertyDto dto);
        Task<ValidationResult> ValidateForDeleteAsync(int id);
    }
}
