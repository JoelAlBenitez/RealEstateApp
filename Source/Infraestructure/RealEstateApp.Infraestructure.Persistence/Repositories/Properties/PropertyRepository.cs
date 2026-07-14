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

        public async Task<IReadOnlyCollection<Property>> GetPropertiesByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Properties
                .AsNoTracking()
                .Include(p => p.Images)
                .Where(p => p.AgentId == agentId)
                .OrderByDescending(p => p.CreateAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Property>> GetFilteredPropertiesAsync(PropertyFilterCriteria criteria)
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
                .ToListAsync();
        }

        public async Task<Property?> GetByIdWithImagesAsync(int id)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.PropertyImprovements)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task DeletePropertiesByAgentAsync(string agentId)
        {
            await _context.Properties
                .Where(p => p.AgentId == agentId)
                .ExecuteDeleteAsync();
        }

        public async Task DeletePropertiesByPropertyTypeAsync(int propertyTypeId)
        {
            await _context.Properties
                .Where(p => p.PropertyTypeId == propertyTypeId)
                .ExecuteDeleteAsync();
        }

        public async Task DeletePropertiesBySaleTypeAsync(int saleTypeId)
        {
            await _context.Properties
                .Where(p => p.SaleTypeId == saleTypeId)
                .ExecuteDeleteAsync();
        }
    }
}
