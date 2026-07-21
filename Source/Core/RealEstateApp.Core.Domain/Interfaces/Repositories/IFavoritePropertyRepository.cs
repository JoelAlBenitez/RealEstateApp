using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;

namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IFavoritePropertyRepository : IGenericRepository<FavoriteProperty, int>
    {
        Task<FavoriteProperty?> GetFavoriteAsync(string customerId, int propertyId);
        Task<IReadOnlyCollection<FavoriteProperty>> GetFavoritesByCustomerAsync(string customerId);
    }
}
