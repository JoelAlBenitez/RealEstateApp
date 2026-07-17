using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Application.Services.Properties
{
    public sealed class PropertyService : GenericServices<SavePropertyDto, Property, int>, IPropertyService
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
            try
            {
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
                var validation = await _validationService.ValidateForDeleteAsync(id);
                if (!validation.IsValid)
                {
                    return validation;
                }
                return await base.RemoveAsync(id);
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableAsync(PropertyFilterDto? filters, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IReadOnlyCollection<Property> properties;

                if (filters != null && (filters.MinPrice.HasValue || filters.MaxPrice.HasValue || filters.Bedrooms.HasValue || filters.Bathrooms.HasValue))
                {
                    var criteria = _mapper.Map<PropertyFilterCriteria>(filters);
                    properties = await _propertyRepository.GetFilteredPropertiesAsync(criteria, pageNumber, pageSize);
                }
                else
                {
                    properties = await _propertyRepository.GetAvailablePropertiesAsync(pageNumber, pageSize);
                }

                var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<PropertyDto>> GetByCodeAsync(string code)
        {
            try
            {
                var property = await _propertyRepository.GetAvailablePropertyByCodeAsync(code);
                if (property == null)
                {
                    return ValidationResult<PropertyDto>.Failure(Domain.Common.CodeErrors.Property.PropertyErrors.NotFoundByCode);
                }
                var dto = _mapper.Map<PropertyDto>(property);
                return ValidationResult<PropertyDto>.Success(dto);
            }
            catch (Exception)
            {
                return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<PropertyDto>> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                var property = await _propertyRepository.GetByIdAsync(id);
                if (property == null)
                {
                    return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Property.NotFound", "La propiedad no existe.") });
                }
                var dto = _mapper.Map<PropertyDto>(property);
                return ValidationResult<PropertyDto>.Success(dto);
            }
            catch (Exception)
            {
                return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var properties = await _propertyRepository.GetAvailablePropertiesByAgentAsync(agentId, pageNumber, pageSize);
                var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllWithDetailsAsync()
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<PropertyTotalsDto>> GetTotalsByStatusAsync()
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var totals = new PropertyTotalsDto
                {
                    AvailableProperties = properties.Count(p => p.Status == PropertyState.Available),
                    SoldProperties = properties.Count(p => p.Status == PropertyState.Sold)
                };
                return ValidationResult<PropertyTotalsDto>.Success(totals);
            }
            catch (Exception)
            {
                return ValidationResult<PropertyTotalsDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<int>> CountByAgentAsync(string agentId)
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var count = properties.Count(p => p.AgentId == agentId);
                return ValidationResult<int>.Success(count);
            }
            catch (Exception)
            {
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult> DeletePropertiesByAgentAsync(string agentId)
        {
            try
            {
                await _propertyRepository.DeletePropertiesByAgentAsync(agentId);
                return ValidationResult.Success();
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<bool> IsAvailableAsync(int propertyId)
        {
            try
            {
                var property = await _propertyRepository.GetByIdAsync(propertyId);
                return property != null && property.Status == PropertyState.Available;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
