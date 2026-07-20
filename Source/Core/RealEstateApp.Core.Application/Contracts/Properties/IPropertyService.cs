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
        Task<ValidationResult<int>> GetAvailableCountAsync(PropertyFilterDto? filters);
        Task<ValidationResult<PropertyDto>> GetByCodeAsync(string code);
        Task<ValidationResult<PropertyDto>> GetByIdWithDetailsAsync(int id);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetPropertiesByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10);
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllWithDetailsAsync();
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllForApiAsync();
        Task<ValidationResult<PropertyDto>> GetByCodeForApiAsync(string code);
        Task<ValidationResult<PropertyTotalsDto>> GetTotalsByStatusAsync();
        Task<ValidationResult<int>> CountByAgentAsync(string agentId);
        Task<ValidationResult<int>> CountByAgentAndStatusAsync(string agentId, RealEstateApp.Core.Domain.Common.Enums.PropertyStatus.PropertyState status);
        Task<bool> IsAvailableAsync(int propertyId);
        Task<ValidationResult> DeletePropertiesByAgentAsync(string agentId);
        Task<ValidationResult<int>> CountByPropertyTypeAsync(int propertyTypeId);
        Task<ValidationResult<int>> CountBySaleTypeAsync(int saleTypeId);
        Task<ValidationResult<int>> CountByImprovementAsync(int improvementId);
        Task<ValidationResult<IReadOnlyCollection<int>>> GetPropertyIdsByTypeAsync(int propertyTypeId);
        Task<ValidationResult<IReadOnlyCollection<int>>> GetPropertyIdsBySaleTypeAsync(int saleTypeId);
        Task<ValidationResult> DeletePropertiesByTypeAsync(int propertyTypeId);
        Task<ValidationResult> DeletePropertiesBySaleTypeAsync(int saleTypeId);
    }
}
