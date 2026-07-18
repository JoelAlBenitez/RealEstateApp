using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infraestructure.Persistence.Repositories.Offers
{
    public sealed class OfferRepository : GenericRepository<Offer, int>, IOfferRepository
    {
        public OfferRepository(DbContextRealEstateApp context) : base(context) { }

        public async Task<Offer?> GetPendingOfferByClientAndPropertyAsync(string customerId, int propertyId)
        {
            return await _context.Offers
                .FirstOrDefaultAsync(o => o.CustomerId == customerId && o.PropertyId == propertyId && o.Status == OfferState.Pending);
        }

        public async Task<bool> HasAcceptedOfferAsync(int propertyId)
        {
            return await _context.Offers
                .AnyAsync(o => o.PropertyId == propertyId && o.Status == OfferState.Accepted);
        }

        public async Task<IReadOnlyCollection<Offer>> GetOffersByClientAsync(string customerId)
        {
            return await _context.Offers
                .AsNoTracking()
                .Include(o => o.Property)
                    .ThenInclude(p => p!.Images)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Offer>> GetPendingOffersByPropertyAsync(int propertyId)
        {
            return await _context.Offers
                .AsNoTracking()
                .Include(o => o.Property)
                .Where(o => o.PropertyId == propertyId && o.Status == OfferState.Pending)
                .ToListAsync();
        }

        public async Task RejectOtherOffersByPropertyAsync(int propertyId, int acceptedOfferId)
        {
            var query = _context.Offers
                .Where(o => o.PropertyId == propertyId && o.Id != acceptedOfferId && o.Status == OfferState.Pending);

            if (_context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                var otherOffers = await query.ToListAsync();
                foreach (var offer in otherOffers)
                {
                    offer.Status = OfferState.Rejected;
                    _context.Entry(offer).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                await query.ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, OfferState.Rejected));
            }
        }

        public async Task<IReadOnlyCollection<Offer>> GetOffersByClientAndPropertyAsync(string customerId, int propertyId)
        {
            return await _context.Offers
                .AsNoTracking()
                .Where(o => o.CustomerId == customerId && o.PropertyId == propertyId)
                .ToListAsync();
        }

        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
