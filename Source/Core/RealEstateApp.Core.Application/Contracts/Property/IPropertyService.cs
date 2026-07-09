using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Property
{
    public interface IPropertyService : IGenericServices<SavePropertyDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableAsync(PropertyFilterDto? filters);
        Task<ValidationResult<PropertyDto>> GetByCodeAsync(string code);
        Task<ValidationResult<PropertyDto>> GetByIdWithDetailsAsync(int id);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableByAgentAsync(string agentId);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllWithDetailsAsync();
        Task<ValidationResult<PropertyTotalsDto>> GetTotalsByStatusAsync();
        Task<ValidationResult<int>> CountByAgentAsync(string agentId);
        Task<ValidationResult> DeletePropertiesByAgentAsync(string agentId);
    }
}
