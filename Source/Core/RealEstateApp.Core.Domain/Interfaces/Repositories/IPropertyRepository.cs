using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;

namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property, int>
    {
        Task<IReadOnlyCollection<Property>> GetAvailablePropertiesAsync();
        Task<IReadOnlyCollection<Property>> GetAvailablePropertiesByAgentAsync(string agentId);
        Task<Property?> GetAvailablePropertyByCodeAsync(string code);
        Task<IReadOnlyCollection<Property>> GetFilteredPropertiesAsync(PropertyFilterCriteria criteria);
        Task<Property?> GetByIdWithImagesAsync(int id);
        Task DeletePropertiesByAgentAsync(string agentId);
    }
}
