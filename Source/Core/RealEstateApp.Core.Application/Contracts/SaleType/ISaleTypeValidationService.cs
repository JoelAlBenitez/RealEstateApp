using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.DTOs.SaleType;

namespace RealEstateApp.Core.Application.Contracts.SaleType
{
    public interface ISaleTypeValidationService
    {
        Task<ValidationResult> ValidateForCreateAsync(SaveSaleTypeDto dto);
        Task<ValidationResult> ValidateForUpdateAsync(SaveSaleTypeDto dto);
    }
}
