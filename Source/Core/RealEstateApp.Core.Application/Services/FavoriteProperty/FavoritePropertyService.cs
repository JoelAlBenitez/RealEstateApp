using AutoMapper;
using RealEstateApp.Core.Application.Contracts.FavoriteProperty;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;

namespace RealEstateApp.Core.Application.Services.FavoriteProperty
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
            dto.CustomerId = _userSession.GetIdCurrentUser();
            var validation = await _validationService.ValidateForCreateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await base.AddAsync(dto);
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            var entity = await _favoritePropertyRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return ValidationResult.Failure(new Domain.Common.Errors.Error("Favorito.NoEncontrado", "El favorito no existe."));
            }
            return await base.RemoveAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<FavoritePropertyDto>>> GetByCustomerAsync()
        {
            var customerId = _userSession.GetIdCurrentUser();
            var favorites = await _favoritePropertyRepository.GetFavoritesByCustomerAsync(customerId);
            var availableFavorites = favorites.Where(f => f.Property != null && f.Property.Status == PropertyState.Available).ToList();
            var dtos = _mapper.Map<IReadOnlyCollection<FavoritePropertyDto>>(availableFavorites);
            return ValidationResult<IReadOnlyCollection<FavoritePropertyDto>>.Success(dtos);
        }

        public async Task<ValidationResult> RemoveFavoriteAsync(int propertyId)
        {
            var validation = await _validationService.ValidateForDeleteAsync(propertyId);
            if (!validation.IsValid)
            {
                return validation;
            }

            var customerId = _userSession.GetIdCurrentUser();
            var favorite = await _favoritePropertyRepository.GetFavoriteAsync(customerId, propertyId);
            if (favorite != null)
            {
                await _favoritePropertyRepository.DeleteAsync(favorite);
                var result = await _favoritePropertyRepository.SaveAsync();
                if (result <= 0)
                {
                    return ValidationResult.Failure(new Domain.Common.Errors.Error("Oops", "Ocurrió un error al eliminar el favorito. Inténtalo de nuevo más tarde."));
                }
            }

            return ValidationResult.Success();
        }
    }
}
