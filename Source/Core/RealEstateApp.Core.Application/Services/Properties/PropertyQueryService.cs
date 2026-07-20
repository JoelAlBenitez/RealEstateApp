using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.Properties
{
    public sealed class PropertyQueryService : IPropertyQueryService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyQueryService> _logger;
        private readonly IOperationalAccountWebApp _accountWebApp;

        public PropertyQueryService(
            IPropertyRepository propertyRepository,
            IMapper mapper,
            ILogger<PropertyQueryService> logger,
            IOperationalAccountWebApp accountWebApp)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _logger = logger;
            _accountWebApp = accountWebApp;
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableAsync(PropertyFilterDto? filters, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IReadOnlyCollection<Domain.Entities.Property> properties;

                if (filters != null && (filters.MinPrice.HasValue || filters.MaxPrice.HasValue || filters.Bedrooms.HasValue || filters.Bathrooms.HasValue || filters.PropertyTypeId.HasValue))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyQueryService");
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<int>> CountAvailableAsync(PropertyFilterDto? filters)
        {
            try
            {
                int total;
                if (filters != null && (filters.MinPrice.HasValue || filters.MaxPrice.HasValue || filters.Bedrooms.HasValue || filters.Bathrooms.HasValue))
                {
                    var criteria = _mapper.Map<PropertyFilterCriteria>(filters);
                    total = await _propertyRepository.CountFilteredPropertiesAsync(criteria);
                }
                else
                {
                    total = await _propertyRepository.CountAvailablePropertiesAsync();
                }
                return ValidationResult<int>.Success(total);
            }
            catch (Exception)
            {
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<int>> GetAvailableCountAsync(PropertyFilterDto? filters)
        {
            try
            {
                PropertyFilterCriteria? criteria = null;
                if (filters != null)
                {
                    criteria = _mapper.Map<PropertyFilterCriteria>(filters);
                }
                var count = await _propertyRepository.GetAvailablePropertiesCountAsync(criteria);
                return ValidationResult<int>.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyQueryService.GetAvailableCountAsync");
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento.") });
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyQueryService");
                return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<PropertyDto>> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                var property = await _propertyRepository.GetByIdWithImagesAsync(id);
                if (property == null)
                {
                    return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Property.NotFound", "La propiedad no existe.") });
                }
                var dto = _mapper.Map<PropertyDto>(property);

                if (!string.IsNullOrEmpty(dto.AgentId))
                {
                    try
                    {
                        var agent = await _accountWebApp.GetConsultAgentById(dto.AgentId);
                        if (agent != null)
                        {
                            dto.AgentName = $"{agent.Name} {agent.LastName}";
                            dto.AgentPhone = agent.PhoneNumber;
                            dto.AgentEmail = agent.Email;
                            dto.AgentPhotoUrl = agent.ProfileImgAgent;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"No se pudo obtener la información de identidad del agente con id {dto.AgentId}");
                    }
                }

                return ValidationResult<PropertyDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyQueryService");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyQueryService");
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<int>> CountAvailableByAgentAsync(string agentId)
        {
            try
            {
                var count = await _propertyRepository.GetPropertiesCountByAgentAsync(agentId, PropertyState.Available);
                return ValidationResult<int>.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyQueryService.CountAvailableByAgentAsync");
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyQueryService");
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<bool> IsAvailableAsync(int propertyId)
        {
            try
            {
                var property = await _propertyRepository.GetByIdAsync(propertyId);
                return property != null && property.Status == PropertyState.Available;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyQueryService");
                return false;
            }
        }
    }
}
