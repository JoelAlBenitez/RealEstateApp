using AutoMapper;
using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.Contracts.Property;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;

namespace RealEstateApp.Core.Application.Services.Property
{
    public sealed class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly IGenericServices<SavePropertyDto, int> _genericService;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IPropertyValidationService validationService,
            IMapper mapper,
            IGenericServices<SavePropertyDto, int> genericService)
        {
            _propertyRepository = propertyRepository;
            _validationService = validationService;
            _mapper = mapper;
            _genericService = genericService;
        }

        public async Task<ValidationResult> AddAsync(SavePropertyDto dto)
        {
            var validation = await _validationService.ValidateForCreateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await _genericService.AddAsync(dto);
        }

        public async Task<ValidationResult?> UpdateAsync(SavePropertyDto dto, int id)
        {
            var validation = await _validationService.ValidateForUpdateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await _genericService.UpdateAsync(dto, id);
        }

        public async Task<ValidationResult<SavePropertyDto>> GetByIdAsync(int id)
        {
            return await _genericService.GetByIdAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<SavePropertyDto>>> GetAllAsync()
        {
            return await _genericService.GetAllAsync();
        }

        public async Task<ValidationResult> DeleteAsync(int id)
        {
            var validation = await _validationService.ValidateForDeleteAsync(id);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await _genericService.DeleteAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableAsync(PropertyFilterDto? filters)
        {
            var properties = await _propertyRepository.GetAvailablePropertiesAsync();
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
            var properties = await _propertyRepository.GetAvailablePropertiesAsync();
            var agentProperties = properties.Where(p => p.AgentId == agentId).ToList();
            var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(agentProperties);
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

            var deleteTasks = agentProperties.Select(p => _propertyRepository.DeleteAsync(p));
            await Task.WhenAll(deleteTasks);

            return ValidationResult.Success();
        }
    }
}
