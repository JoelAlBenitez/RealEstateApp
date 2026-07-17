using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;

namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property, int>
    {
        Task<IReadOnlyCollection<Property>> GetAvailablePropertiesAsync(int pageNumber = 1, int pageSize = 10);
        Task<IReadOnlyCollection<Property>> GetAvailablePropertiesByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10);
        Task<IReadOnlyCollection<Property>> GetPropertiesByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10);
        Task<Property?> GetAvailablePropertyByCodeAsync(string code);
        Task<bool> ExistsCodeAsync(string code);
        Task<IReadOnlyCollection<Property>> GetFilteredPropertiesAsync(PropertyFilterCriteria criteria, int pageNumber = 1, int pageSize = 10);
        Task<IReadOnlyCollection<Property>> GetFilteredPropertiesAsync(PropertyFilterCriteria criteria);
        Task<Property?> GetByIdWithImagesAsync(int id);
        Task DeletePropertiesByAgentAsync(string agentId);
        Task DeletePropertiesByPropertyTypeAsync(int propertyTypeId);
        Task DeletePropertiesBySaleTypeAsync(int saleTypeId);
        Task<IReadOnlyCollection<int>> GetPropertyIdsByTypeAsync(int propertyTypeId);
        Task<IReadOnlyCollection<int>> GetPropertyIdsBySaleTypeAsync(int saleTypeId);
        Task<IReadOnlyCollection<Property>> GetPropertiesByPropertyTypeWithImagesAsync(int propertyTypeId);
        Task<IReadOnlyCollection<Property>> GetPropertiesBySaleTypeWithImagesAsync(int saleTypeId);
        Task<IReadOnlyCollection<Property>> GetPropertiesByAgentWithImagesAsync(string agentId);
        Task<int> GetAvailablePropertiesCountAsync(PropertyFilterCriteria? criteria = null);
        Task<int> GetPropertiesCountByAgentAsync(string agentId, PropertyState? status = null);
    }
}
