using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infraestructure.Persistence.Repositories.FavoriteProperties
{
    public sealed class FavoritePropertyRepository : GenericRepository<FavoriteProperty, int>, IFavoritePropertyRepository
    {
        public FavoritePropertyRepository(DbContextRealEstateApp context) : base(context) { }

        public async Task<FavoriteProperty?> GetFavoriteAsync(string customerId, int propertyId)
        {
            return await _context.FavoriteProperties
                .FirstOrDefaultAsync(fp => fp.CustomerId == customerId && fp.PropertyId == propertyId);
        }

        public async Task<IReadOnlyCollection<FavoriteProperty>> GetFavoritesByCustomerAsync(string customerId)
        {
            return await _context.FavoriteProperties
                .AsNoTracking()
                .Include(fp => fp.Property)
                    .ThenInclude(p => p!.Images)
                .Where(fp => fp.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
