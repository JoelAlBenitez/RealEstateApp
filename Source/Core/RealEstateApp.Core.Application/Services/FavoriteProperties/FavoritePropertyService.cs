using AutoMapper;
using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Application.Services.FavoriteProperties
{
    public sealed class FavoritePropertyService : GenericServices<SaveFavoritePropertyDto, Domain.Entities.FavoriteProperty, int>, IFavoritePropertyService
    {
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;
        private readonly IFavoritePropertyValidationService _validationService;
        private readonly IUserSession _userSession;

        public FavoritePropertyService(
            IFavoritePropertyRepository favoritePropertyRepository,
            IFavoritePropertyValidationService validationService,
            IMapper mapper,
            IUserSession userSession)
            : base(favoritePropertyRepository, mapper)
        {
            _favoritePropertyRepository = favoritePropertyRepository;
            _validationService = validationService;
            _userSession = userSession;
        }

        public override async Task<ValidationResult> AddAsync(SaveFavoritePropertyDto dto)
        {
            try
            {
                dto.CustomerId = _userSession.GetIdCurrentUser();
                var validation = await _validationService.ValidateForCreateAsync(dto);
                if (!validation.IsValid)
                {
                    return validation;
                }
                return await base.AddAsync(dto);
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            try
            {
                var entity = await _favoritePropertyRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return ValidationResult.Failure(new Error("Favorite.NotFound", "El favorito no existe."));
                }
                return await base.RemoveAsync(id);
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<FavoritePropertyDto>>> GetByCustomerAsync()
        {
            try
            {
                var customerId = _userSession.GetIdCurrentUser();
                var favorites = await _favoritePropertyRepository.GetFavoritesByCustomerAsync(customerId);
                var availableFavorites = favorites.Where(f => f.Property != null && f.Property.Status == PropertyState.Available).ToList();
                var dtos = _mapper.Map<IReadOnlyCollection<FavoritePropertyDto>>(availableFavorites);
                return ValidationResult<IReadOnlyCollection<FavoritePropertyDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<FavoritePropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult> RemoveFavoriteAsync(int propertyId)
        {
            try
            {
                var validation = await _validationService.ValidateForDeleteAsync(propertyId);
                if (!validation.IsValid)
                {
                    return validation;
                }

                var customerId = _userSession.GetIdCurrentUser();
                var favorite = await _favoritePropertyRepository.GetFavoriteAsync(customerId, propertyId);
                if (favorite == null)
                {
                    return ValidationResult.Failure(new Error("Favorite.NotFound", "La propiedad favorita especificada no existe."));
                }

                await _favoritePropertyRepository.DeleteAsync(favorite);
                var result = await _favoritePropertyRepository.SaveAsync();
                if (result <= 0)
                {
                    return ValidationResult.Failure(new Error("Oops", "Ocurrió un error al eliminar el favorito. Inténtalo de nuevo más tarde."));
                }

                return ValidationResult.Success();
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }
    }
}
