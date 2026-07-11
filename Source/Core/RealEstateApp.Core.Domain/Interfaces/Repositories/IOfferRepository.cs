using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;

namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IOfferRepository : IGenericRepository<Offer, int>
    {
        Task<Offer?> GetPendingOfferByClientAndPropertyAsync(string customerId, int propertyId);
        Task<bool> HasAcceptedOfferAsync(int propertyId);
        Task<IReadOnlyCollection<Offer>> GetOffersByClientAsync(string customerId);
        Task<IReadOnlyCollection<Offer>> GetPendingOffersByPropertyAsync(int propertyId);
        Task RejectOtherOffersByPropertyAsync(int propertyId, int acceptedOfferId);
    }
}
