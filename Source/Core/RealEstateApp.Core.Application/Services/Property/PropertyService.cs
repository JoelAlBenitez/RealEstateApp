using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Property;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Application.Services.Generic;

namespace RealEstateApp.Core.Application.Services.Property
{
    public sealed class PropertyService : GenericServices<SavePropertyDto, Domain.Entities.Property, int>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyValidationService _validationService;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IPropertyValidationService validationService,
            IMapper mapper)
            : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _validationService = validationService;
        }

        public override async Task<ValidationResult> AddAsync(SavePropertyDto dto)
        {
            var validation = await _validationService.ValidateForCreateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await base.AddAsync(dto);
        }

        public override async Task<ValidationResult?> UpdateAsync(SavePropertyDto dto)
        {
            var validation = await _validationService.ValidateForUpdateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await base.UpdateAsync(dto);
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            var validation = await _validationService.ValidateForDeleteAsync(id);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await base.RemoveAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableAsync(PropertyFilterDto? filters)
        {
            IReadOnlyCollection<Domain.Entities.Property> properties;

            if (filters != null && (filters.MinPrice.HasValue || filters.MaxPrice.HasValue || filters.Bedrooms.HasValue || filters.Bathrooms.HasValue))
            {
                var criteria = _mapper.Map<PropertyFilterCriteria>(filters);
                properties = await _propertyRepository.GetFilteredPropertiesAsync(criteria);
            }
            else
            {
                properties = await _propertyRepository.GetAvailablePropertiesAsync();
            }

            var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
            return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
        }

        public async Task<ValidationResult<PropertyDto>> GetByCodeAsync(string code)
        {
            var property = await _propertyRepository.GetAvailablePropertyByCodeAsync(code);
            if (property == null)
            {
                return ValidationResult<PropertyDto>.Failure(Domain.Common.CodeErrors.Property.PropertyErrors.NotFoundByCode);
            }
            var dto = _mapper.Map<PropertyDto>(property);
            return ValidationResult<PropertyDto>.Success(dto);
        }

        public async Task<ValidationResult<PropertyDto>> GetByIdWithDetailsAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
            {
                return ValidationResult<PropertyDto>.Failure(new Domain.Common.Errors.Error("Property.NotFound", "La propiedad no existe"));
            }
            var dto = _mapper.Map<PropertyDto>(property);
            return ValidationResult<PropertyDto>.Success(dto);
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableByAgentAsync(string agentId)
        {
            var properties = await _propertyRepository.GetAvailablePropertiesByAgentAsync(agentId);
            var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
            return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllWithDetailsAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
            return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
        }

        public async Task<ValidationResult<PropertyTotalsDto>> GetTotalsByStatusAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            var totals = new PropertyTotalsDto
            {
                AvailableProperties = properties.Count(p => p.Status == PropertyState.Available),
                SoldProperties = properties.Count(p => p.Status == PropertyState.Sold)
            };
            return ValidationResult<PropertyTotalsDto>.Success(totals);
        }

        public async Task<ValidationResult<int>> CountByAgentAsync(string agentId)
        {
            var properties = await _propertyRepository.GetAllAsync();
            var count = properties.Count(p => p.AgentId == agentId);
            return ValidationResult<int>.Success(count);
        }

        public async Task<ValidationResult> DeletePropertiesByAgentAsync(string agentId)
        {
            var properties = await _propertyRepository.GetAllAsync();
            var agentProperties = properties.Where(p => p.AgentId == agentId).ToList();

            foreach (var p in agentProperties)
            {
                await _propertyRepository.DeleteAsync(p);
            }

            if (agentProperties.Count > 0)
            {
                var result = await _propertyRepository.SaveAsync();
                if (result <= 0)
                {
                    return ValidationResult.Failure(new Domain.Common.Errors.Error("Oops", "Ocurrió un error al eliminar las propiedades. Inténtalo de nuevo más tarde."));
                }
            }

            return ValidationResult.Success();
        }

        public async Task<bool> IsAvailableAsync(int propertyId)
        {
            var property = await _propertyRepository.GetByIdAsync(propertyId);
            return property != null && property.Status == PropertyState.Available;
        }
    }
}
