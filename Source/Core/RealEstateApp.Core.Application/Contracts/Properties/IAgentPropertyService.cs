using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Properties
{
    public interface IAgentPropertyService : IGenericServices<SavePropertyDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetPropertiesByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10);
        Task<ValidationResult<int>> CountByAgentAsync(string agentId);
        Task<ValidationResult<int>> CountByAgentAndStatusAsync(string agentId, RealEstateApp.Core.Domain.Common.Enums.PropertyStatus.PropertyState status);
    }
}
