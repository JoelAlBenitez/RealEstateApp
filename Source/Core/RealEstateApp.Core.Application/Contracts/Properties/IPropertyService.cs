using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Properties
{
    public interface IPropertyService : IGenericServices<SavePropertyDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableAsync(PropertyFilterDto? filters, int pageNumber = 1, int pageSize = 10);
        Task<ValidationResult<int>> CountAvailableAsync(PropertyFilterDto? filters);
        Task<ValidationResult<int>> CountAvailableByAgentAsync(string agentId);
        Task<ValidationResult<PropertyDto>> GetByCodeAsync(string code);
        Task<ValidationResult<PropertyDto>> GetByIdWithDetailsAsync(int id);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllWithDetailsAsync();
        Task<ValidationResult<PropertyTotalsDto>> GetTotalsByStatusAsync();
        Task<ValidationResult<int>> CountByAgentAsync(string agentId);
        Task<bool> IsAvailableAsync(int propertyId);
        Task<ValidationResult> DeletePropertiesByAgentAsync(string agentId);
    }
}
