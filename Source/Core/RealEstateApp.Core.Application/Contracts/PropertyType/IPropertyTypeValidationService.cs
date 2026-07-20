using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.DTOs.PropertyType;

namespace RealEstateApp.Core.Application.Contracts.PropertyType
{
    public interface IPropertyTypeValidationService
    {
        Task<ValidationResult> ValidateForCreateAsync(SavePropertyTypeDto dto);
        Task<ValidationResult> ValidateForUpdateAsync(SavePropertyTypeDto dto);
    }
}
