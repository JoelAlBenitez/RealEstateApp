using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.Properties
{
    public sealed class PropertyApiQueryService : IPropertyApiQueryService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyApiQueryService> _logger;
        private readonly IOperationalAccountWebApp _accountWebApp;

        public PropertyApiQueryService(
            IPropertyRepository propertyRepository,
            IMapper mapper,
            ILogger<PropertyApiQueryService> logger,
            IOperationalAccountWebApp accountWebApp)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _logger = logger;
            _accountWebApp = accountWebApp;
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllForApiAsync()
        {
            try
            {
                var properties = await _propertyRepository.GetAllWithImagesAsync();
                var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
                await FillAgentNamesAsync(dtos);
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyApiQueryService.GetAllForApiAsync");
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<PropertyDto>> GetByCodeForApiAsync(string code)
        {
            try
            {
                var property = await _propertyRepository.GetByCodeAsync(code);
                if (property == null)
                {
                    return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Property.NotFound", "No existe una propiedad registrada con el código enviado.") });
                }
                var dto = _mapper.Map<PropertyDto>(property);
                await FillAgentNamesAsync(new List<PropertyDto> { dto });
                return ValidationResult<PropertyDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyApiQueryService.GetByCodeForApiAsync");
                return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        private async Task FillAgentNamesAsync(IReadOnlyCollection<PropertyDto> dtos)
        {
            var agentIds = dtos
                .Where(d => !string.IsNullOrEmpty(d.AgentId))
                .Select(d => d.AgentId)
                .Distinct()
                .ToList();

            var agentNames = new Dictionary<string, string>();
            foreach (var agentId in agentIds)
            {
                try
                {
                    var agent = await _accountWebApp.GetUserBaseById(agentId);
                    if (agent != null)
                    {
                        agentNames[agentId] = $"{agent.Name} {agent.LastName}";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"No se pudo obtener la información del agente con id {agentId}");
                }
            }

            foreach (var dto in dtos)
            {
                if (agentNames.TryGetValue(dto.AgentId, out var name))
                {
                    dto.AgentName = name;
                }
            }
        }
    }
}
