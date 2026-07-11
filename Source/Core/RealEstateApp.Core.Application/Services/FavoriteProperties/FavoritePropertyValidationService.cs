using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Domain.Common.CodeErrors.Favorite;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;

namespace RealEstateApp.Core.Application.Services.FavoriteProperties
{
    public class FavoritePropertyValidationService : IFavoritePropertyValidationService
    {
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;
        private readonly IPropertyService _propertyService;
        private readonly IUserSession _userSession;

        public FavoritePropertyValidationService(
            IFavoritePropertyRepository favoritePropertyRepository,
            IPropertyService propertyService,
            IUserSession userSession)
        {
            _favoritePropertyRepository = favoritePropertyRepository;
            _propertyService = propertyService;
            _userSession = userSession;
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SaveFavoritePropertyDto dto)
        {
            var errors = new List<Error>();

            var isAvailable = await _propertyService.IsAvailableAsync(dto.PropertyId);
            if (!isAvailable)
            {
                errors.Add(new Error("Favorito.PropiedadNoEncontrada", "La propiedad especificada no existe o no está disponible."));
                return ValidationResult.Failure(errors);
            }

            if (!string.IsNullOrEmpty(dto.CustomerId))
            {
                var favorites = await _favoritePropertyRepository.GetFavoritesByCustomerAsync(dto.CustomerId);
                if (favorites.Any(f => f.PropertyId == dto.PropertyId))
                {
                    errors.Add(FavoriteErrors.AlreadyFavorite);
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForDeleteAsync(int propertyId)
        {
            var errors = new List<Error>();
            var customerId = _userSession.GetIdCurrentUser();

            var favorites = await _favoritePropertyRepository.GetFavoritesByCustomerAsync(customerId);
            if (!favorites.Any(f => f.PropertyId == propertyId))
            {
                errors.Add(new Error("Favorito.NoEncontrado", "La propiedad no se encuentra en su listado de favoritos."));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
