using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infraestructure.Persistence.Repositories.Properties
{
    public sealed class PropertyRepository : GenericRepository<Property, int>, IPropertyRepository
    {
        public PropertyRepository(DbContextRealEstateApp context) : base(context) { }


        public override async Task<IReadOnlyCollection<Property>> GetAllAsync()
        {
            return await _context.Properties
                 .AsNoTracking()
                 .Where(p => p.Status == PropertyState.Available)
                 .Include(i => i.Images)
                 .OrderByDescending(p => p.CreateAt)
                 .ToListAsync();
           
        }

        public async Task<IReadOnlyCollection<Property>> GetAvailablePropertiesAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Properties
                .AsNoTracking()
                .Include(p => p.Images)
                .Where(p => p.Status == PropertyState.Available)
                .OrderByDescending(p => p.CreateAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Property>> GetAvailablePropertiesByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Properties
                .AsNoTracking()
                .Include(p => p.Images)
                .Where(p => p.AgentId == agentId && p.Status == PropertyState.Available)
                .OrderByDescending(p => p.CreateAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Property?> GetAvailablePropertyByCodeAsync(string code)
        {
            return await _context.Properties
                .AsNoTracking()
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Code == code && p.Status == PropertyState.Available);
        }

        public async Task<IReadOnlyCollection<Property>> GetFilteredPropertiesAsync(PropertyFilterCriteria criteria, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Properties
                .AsNoTracking()
                .Include(p => p.Images)
                .Where(p => p.Status == PropertyState.Available);

            if (criteria.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= criteria.MinPrice.Value);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= criteria.MaxPrice.Value);
            }

            if (criteria.Bedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms == criteria.Bedrooms.Value);
            }

            if (criteria.Bathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms == criteria.Bathrooms.Value);
            }

            return await query
                .OrderByDescending(p => p.CreateAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAvailablePropertiesAsync()
        {
            return await _context.Properties
                .AsNoTracking()
                .CountAsync(p => p.Status == PropertyState.Available);
        }

        public async Task<int> CountAvailablePropertiesByAgentAsync(string agentId)
        {
            return await _context.Properties
                .AsNoTracking()
                .CountAsync(p => p.AgentId == agentId && p.Status == PropertyState.Available);
        }

        public async Task<int> CountFilteredPropertiesAsync(PropertyFilterCriteria criteria)
        {
            var query = _context.Properties
                .AsNoTracking()
                .Where(p => p.Status == PropertyState.Available);

            if (criteria.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= criteria.MinPrice.Value);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= criteria.MaxPrice.Value);
            }

            if (criteria.Bedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms == criteria.Bedrooms.Value);
            }

            if (criteria.Bathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms == criteria.Bathrooms.Value);
            }

            return await query.CountAsync();
        }

        public async Task DeletePropertiesByAgentAsync(string agentId)
        {
            await _context.Properties
                .Where(p => p.AgentId == agentId)
                .ExecuteDeleteAsync();
        }
    }
}
