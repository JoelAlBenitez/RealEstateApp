using RealEstateApp.Core.Application.DTOs.Api.Agents;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Api
{
    public interface IAgentApiService
    {
        Task<IReadOnlyCollection<AgentApiDto>> GetAllAsync();
        Task<AgentApiDto?> GetByIdAsync(string id);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>?> GetAgentPropertiesAsync(string agentId);
        Task<ChangeAgentStatusResult> ChangeStatusAsync(string agentId, bool status);
    }
}
