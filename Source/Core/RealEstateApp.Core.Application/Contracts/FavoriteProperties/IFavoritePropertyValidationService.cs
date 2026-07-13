using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.FavoriteProperties
{
    public interface IFavoritePropertyValidationService
    {
        Task<ValidationResult> ValidateForCreateAsync(SaveFavoritePropertyDto dto);
        Task<ValidationResult> ValidateForDeleteAsync(int propertyId);
    }
}
