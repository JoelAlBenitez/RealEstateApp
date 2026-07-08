using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;

namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IOfferRepository : IGenericRepository<Offer, int>
    {
        Task<Offer?> GetPendingOfferByClientAndPropertyAsync(string clientId, int propertyId);
        Task<bool> HasAcceptedOfferAsync(int propertyId);
        Task<IReadOnlyCollection<Offer>> GetOffersByClientAsync(string clientId);
        Task<IReadOnlyCollection<Offer>> GetPendingOffersByPropertyAsync(int propertyId);
    }
}
