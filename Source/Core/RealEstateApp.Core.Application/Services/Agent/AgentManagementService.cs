using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Agent;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Common.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services.Agent
{
    // Servicio para la pantalla de listado de agentes
    public sealed class AgentManagementService : IAgentManagementService
    {
        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApi _internalAccountApi;

        public AgentManagementService(IPropertyService propertyService, IOperationalAccountWebApi internalAccountApi)
        {
            _propertyService = propertyService;
            _internalAccountApi = internalAccountApi;
        }

        public async Task<ValidationResult<IReadOnlyCollection<AdminConsultAgentDto>>> GetAgentsAsync()
        {
            var agents = await _internalAccountApi.GetAllAgentesByConsultAdmin();
            foreach (var agent in agents)
            {
                var countResult = await _propertyService.CountByAgentAsync(agent.Id);
                if (countResult.IsValid)
                {
                    agent.Properties = countResult.Value;
                }
            }
            return ValidationResult<IReadOnlyCollection<AdminConsultAgentDto>>.Success(agents);
        }

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto)
        {
            var result = await _internalAccountApi.ChangeStateAsync(dto);
            if (result.HasError)
            {
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            }
            return ValidationResult.Success();
        }

        public async Task<ValidationResult> DeleteAgentAsync(string agentId)
        {
            var agent = await _internalAccountApi.GetUserBaseById(agentId);
            if (agent == null)
            {
                return ValidationResult.Failure(ErrorAgent.NotFound);
            }

            var cascadeResult = await _propertyService.DeletePropertiesByAgentAsync(agentId);
            if (!cascadeResult.IsValid)
            {
                return cascadeResult;
            }

            var result = await _internalAccountApi.DeleteAsync(agentId);
            if (result.HasError)
            {
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            }

            return ValidationResult.Success();
        }
    }
}
