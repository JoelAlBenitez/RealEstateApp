using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.FavoriteProperty
{
    public interface IFavoritePropertyService : IGenericServices<SaveFavoritePropertyDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<FavoritePropertyDto>>> GetByCustomerAsync();
        Task<ValidationResult> RemoveFavoriteAsync(int propertyId);
    }
}
