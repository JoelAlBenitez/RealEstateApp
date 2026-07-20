using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Properties
{
    public interface IPropertyQueryService
    {
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableAsync(PropertyFilterDto? filters, int pageNumber = 1, int pageSize = 10);
        Task<ValidationResult<int>> CountAvailableAsync(PropertyFilterDto? filters);
        Task<ValidationResult<int>> GetAvailableCountAsync(PropertyFilterDto? filters);
        Task<ValidationResult<PropertyDto>> GetByCodeAsync(string code);
        Task<ValidationResult<PropertyDto>> GetByIdWithDetailsAsync(int id);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10);
        Task<ValidationResult<int>> CountAvailableByAgentAsync(string agentId);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllWithDetailsAsync();
        Task<bool> IsAvailableAsync(int propertyId);
    }
}
