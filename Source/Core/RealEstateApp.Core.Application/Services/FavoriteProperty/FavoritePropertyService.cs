using AutoMapper;
using RealEstateApp.Core.Application.Contracts.FavoriteProperty;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Application.Services.Generic;

namespace RealEstateApp.Core.Application.Services.FavoriteProperty
{
    public sealed class FavoritePropertyService : GenericServices<SaveFavoritePropertyDto, Domain.Entities.FavoriteProperty, int>, IFavoritePropertyService
    {
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;
        private readonly IFavoritePropertyValidationService _validationService;

        public FavoritePropertyService(
            IFavoritePropertyRepository favoritePropertyRepository,
            IFavoritePropertyValidationService validationService,
            IMapper mapper)
            : base(favoritePropertyRepository, mapper)
        {
            _favoritePropertyRepository = favoritePropertyRepository;
            _validationService = validationService;
        }

        public override async Task<ValidationResult> AddAsync(SaveFavoritePropertyDto dto)
        {
            var validation = await _validationService.ValidateForCreateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await base.AddAsync(dto);
        }

        public override async Task<ValidationResult?> UpdateAsync(SaveFavoritePropertyDto dto)
        {
            return await base.UpdateAsync(dto);
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            var entity = await _favoritePropertyRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return ValidationResult.Failure(new Domain.Common.Errors.Error("Favorite.NotFound", "El favorito no existe"));
            }
            return await base.RemoveAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<FavoritePropertyDto>>> GetByCustomerAsync(string customerId)
        {
            var favorites = await _favoritePropertyRepository.GetFavoritesByCustomerAsync(customerId);
            var availableFavorites = favorites.Where(f => f.Property != null && f.Property.Status == PropertyState.Available).ToList();
            var dtos = _mapper.Map<IReadOnlyCollection<FavoritePropertyDto>>(availableFavorites);
            return ValidationResult<IReadOnlyCollection<FavoritePropertyDto>>.Success(dtos);
        }

        public async Task<ValidationResult> RemoveFavoriteAsync(string customerId, int propertyId)
        {
            var validation = await _validationService.ValidateForDeleteAsync(customerId, propertyId);
            if (!validation.IsValid)
            {
                return validation;
            }

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
