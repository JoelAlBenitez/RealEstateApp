using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Properties
{
    public interface IPropertyCascadeService
    {
        Task<ValidationResult<PropertyTotalsDto>> GetTotalsByStatusAsync();
        Task<ValidationResult> DeletePropertiesByAgentAsync(string agentId);
        Task<ValidationResult<int>> CountByPropertyTypeAsync(int propertyTypeId);
        Task<ValidationResult<int>> CountBySaleTypeAsync(int saleTypeId);
        Task<ValidationResult<IReadOnlyDictionary<int, int>>> GetPropertyCountsByTypeAsync();
        Task<ValidationResult<IReadOnlyDictionary<int, int>>> GetPropertyCountsBySaleTypeAsync();
        Task<ValidationResult<int>> CountByImprovementAsync(int improvementId);
        Task<ValidationResult<IReadOnlyCollection<int>>> GetPropertyIdsByTypeAsync(int propertyTypeId);
        Task<ValidationResult<IReadOnlyCollection<int>>> GetPropertyIdsBySaleTypeAsync(int saleTypeId);
        Task<ValidationResult> DeletePropertiesByTypeAsync(int propertyTypeId);
        Task<ValidationResult> DeletePropertiesBySaleTypeAsync(int saleTypeId);
    }
}
