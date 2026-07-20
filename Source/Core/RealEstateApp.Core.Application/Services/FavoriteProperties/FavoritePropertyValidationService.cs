using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Domain.Common.CodeErrors.Favorite;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Core.Application.Services.FavoriteProperties
{
    public class FavoritePropertyValidationService : IFavoritePropertyValidationService
    {
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;
        private readonly IPropertyQueryService _propertyQueryService;
        private readonly IUserSession _userSession;

        public FavoritePropertyValidationService(
            IFavoritePropertyRepository favoritePropertyRepository,
            IPropertyQueryService propertyQueryService,
            IUserSession userSession)
        {
            _favoritePropertyRepository = favoritePropertyRepository;
            _propertyQueryService = propertyQueryService;
            _userSession = userSession;
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SaveFavoritePropertyDto dto)
        {
            var errors = new List<Error>();

            var roles = _userSession.GetRolesCurrentUser();
            if (!roles.Contains(Roles.Cliente.ToString()))
            {
                errors.Add(new Error("Favorito.UsuarioNoAutorizado", "Solo los clientes pueden agregar favoritos."));
                return ValidationResult.Failure(errors);
            }

            var isAvailable = await _propertyQueryService.IsAvailableAsync(dto.PropertyId);
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

            var roles = _userSession.GetRolesCurrentUser();
            if (!roles.Contains(Roles.Cliente.ToString()))
            {
                errors.Add(new Error("Favorito.UsuarioNoAutorizado", "Solo los clientes pueden eliminar favoritos."));
                return ValidationResult.Failure(errors);
            }

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
