using Microsoft.Extensions.Logging;
using RealEstateApp.Core.Application.Contracts.Api;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Api.Agents;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services.Api
{
    public sealed class AgentApiService : IAgentApiService
    {
        private readonly IOperationalAccountWebApi _operationalAccountWebApi;
        private readonly IAgentPropertyService _agentPropertyService;
        private readonly ILogger<AgentApiService> _logger;

        public AgentApiService(
            IOperationalAccountWebApi operationalAccountWebApi,
            IAgentPropertyService agentPropertyService,
            ILogger<AgentApiService> logger)
        {
            _operationalAccountWebApi = operationalAccountWebApi;
            _agentPropertyService = agentPropertyService;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<AgentApiDto>> GetAllAsync()
        {
            var agents = await _operationalAccountWebApi.GetAllAgentsForApiAsync();
            foreach (var agent in agents)
            {
                agent.NumberOfProperties = await CountPropertiesAsync(agent.Id);
            }
            return agents;
        }

        public async Task<AgentApiDto?> GetByIdAsync(string id)
        {
            var agent = await _operationalAccountWebApi.GetAgentByIdForApiAsync(id);
            if (agent == null) return null;
            agent.NumberOfProperties = await CountPropertiesAsync(agent.Id);
            return agent;
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>?> GetAgentPropertiesAsync(string agentId)
        {
            var agent = await _operationalAccountWebApi.GetAgentByIdForApiAsync(agentId);
            if (agent == null) return null;
            return await _agentPropertyService.GetPropertiesByAgentAsync(agentId, 1, int.MaxValue);
        }

        public async Task<ChangeAgentStatusResult> ChangeStatusAsync(string agentId, bool status)
        {
            var agent = await _operationalAccountWebApi.GetAgentByIdForApiAsync(agentId);
            if (agent == null) return ChangeAgentStatusResult.NotFound;

            var result = await _operationalAccountWebApi.ChangeStateAsync(new AlterStateUserDto
            {
                Id = agentId,
                State = status
            });

            if (result == null || result.HasError)
            {
                _logger.LogError("No fue posible cambiar el estado del agente {AgentId}. Errores: {Errors}",
                    agentId, result != null ? string.Join(" | ", result.Errors) : "Sin respuesta");
                return ChangeAgentStatusResult.Error;
            }

            return ChangeAgentStatusResult.Success;
        }

        private async Task<int> CountPropertiesAsync(string agentId)
        {
            var count = await _agentPropertyService.CountByAgentAsync(agentId);
            return count.IsValid ? count.Value : 0;
        }
    }
}
