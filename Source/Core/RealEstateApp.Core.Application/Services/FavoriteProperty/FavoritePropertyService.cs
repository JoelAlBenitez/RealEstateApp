using AutoMapper;
using RealEstateApp.Core.Application.Contracts.FavoriteProperty;
using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.FavoriteProperty;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.FavoriteProperty
{
    public sealed class FavoritePropertyService : IFavoritePropertyService
    {
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;
        private readonly IFavoritePropertyValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly IGenericServices<SaveFavoritePropertyDto, int> _genericService;

        public FavoritePropertyService(
            IFavoritePropertyRepository favoritePropertyRepository,
            IFavoritePropertyValidationService validationService,
            IMapper mapper,
            IGenericServices<SaveFavoritePropertyDto, int> genericService)
        {
            _favoritePropertyRepository = favoritePropertyRepository;
            _validationService = validationService;
            _mapper = mapper;
            _genericService = genericService;
        }

        public async Task<ValidationResult> AddAsync(SaveFavoritePropertyDto dto)
        {
            var validation = await _validationService.ValidateForCreateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await _genericService.AddAsync(dto);
        }

        public async Task<ValidationResult?> UpdateAsync(SaveFavoritePropertyDto dto, int id)
        {
            return await _genericService.UpdateAsync(dto, id);
        }

        public async Task<ValidationResult<SaveFavoritePropertyDto>> GetByIdAsync(int id)
        {
            return await _genericService.GetByIdAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<SaveFavoritePropertyDto>>> GetAllAsync()
        {
            return await _genericService.GetAllAsync();
        }

        public async Task<ValidationResult> DeleteAsync(int id)
        {
            var entity = await _favoritePropertyRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return ValidationResult.Failure(new Domain.Common.Errors.Error("Favorite.NotFound", "El favorito no existe"));
            }
            return await _genericService.DeleteAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<FavoritePropertyDto>>> GetByCustomerAsync(string customerId)
        {
            var favorites = await _favoritePropertyRepository.GetFavoritesByCustomerAsync(customerId);
            var dtos = _mapper.Map<IReadOnlyCollection<FavoritePropertyDto>>(favorites);
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
            }

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> RemoveAsync(int id)
        {
            return await _genericService.RemoveAsync(id);
        }
    }
}
