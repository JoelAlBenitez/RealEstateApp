using RealEstateApp.Core.Application.Contracts.FavoriteProperty;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Domain.Common.CodeErrors.Favorite;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.FavoriteProperty
{
    public class FavoritePropertyValidationService : IFavoritePropertyValidationService
    {
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;
        private readonly IPropertyRepository _propertyRepository;

        public FavoritePropertyValidationService(
            IFavoritePropertyRepository favoritePropertyRepository,
            IPropertyRepository propertyRepository)
        {
            _favoritePropertyRepository = favoritePropertyRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SaveFavoritePropertyDto dto)
        {
            var errors = new List<Error>();

            var property = await _propertyRepository.GetByIdAsync(dto.PropertyId);
            if (property == null)
            {
                errors.Add(new Error("Favorite.PropertyNotFound", "La propiedad especificada no existe"));
                return ValidationResult.Failure(errors);
            }

            if (!string.IsNullOrEmpty(dto.CustomerId))
            {
                var favorites = await _favoritePropertyRepository.GetFavoritesByCustomertAsync(dto.CustomerId);
                if (favorites.Any(f => f.PropertyId == dto.PropertyId))
                {
                    errors.Add(FavoriteErrors.AlreadyFavorite);
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForDeleteAsync(string customerId, int propertyId)
        {
            var errors = new List<Error>();

            var favorites = await _favoritePropertyRepository.GetFavoritesByCustomertAsync(customerId);
            if (!favorites.Any(f => f.PropertyId == propertyId))
            {
                errors.Add(new Error("Favorite.NotFound", "La propiedad no se encuentra en su listado de favoritos"));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
