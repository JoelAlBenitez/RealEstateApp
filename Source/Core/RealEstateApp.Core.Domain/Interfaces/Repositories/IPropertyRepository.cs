using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;

namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property, int>
    {
        Task<IReadOnlyCollection<Property>> GetAvailablePropertiesAsync(int pageNumber = 1, int pageSize = 10);
        Task<IReadOnlyCollection<Property>> GetAvailablePropertiesByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10);
        Task<Property?> GetAvailablePropertyByCodeAsync(string code);
        Task<IReadOnlyCollection<Property>> GetFilteredPropertiesAsync(PropertyFilterCriteria criteria, int pageNumber = 1, int pageSize = 10);
        Task<int> CountAvailablePropertiesAsync();
        Task<int> CountAvailablePropertiesByAgentAsync(string agentId);
        Task<int> CountFilteredPropertiesAsync(PropertyFilterCriteria criteria);
        Task DeletePropertiesByAgentAsync(string agentId);
    }
}
