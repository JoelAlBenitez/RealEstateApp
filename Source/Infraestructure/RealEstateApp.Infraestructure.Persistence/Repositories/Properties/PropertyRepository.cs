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
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
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
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
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
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
                .FirstOrDefaultAsync(p => p.Code == code && p.Status == PropertyState.Available);
        }

        public async Task<bool> ExistsCodeAsync(string code)
        {
            return await _context.Properties.AnyAsync(p => p.Code == code);
        }

        public async Task<IReadOnlyCollection<Property>> GetFilteredPropertiesAsync(PropertyFilterCriteria criteria, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Properties
                .AsNoTracking()
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
                .Where(p => p.Status == PropertyState.Available);

            if (criteria.PropertyTypeId.HasValue)
            {
                query = query.Where(p => p.PropertyTypeId == criteria.PropertyTypeId.Value);
            }

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

        public async Task<IReadOnlyCollection<Property>> GetPropertiesByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Properties
                .AsNoTracking()
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
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
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
                .Where(p => p.Status == PropertyState.Available);

            if (criteria.PropertyTypeId.HasValue)
            {
                query = query.Where(p => p.PropertyTypeId == criteria.PropertyTypeId.Value);
            }

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
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
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
        public async Task<IReadOnlyCollection<int>> GetPropertyIdsByTypeAsync(int propertyTypeId)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.PropertyTypeId == propertyTypeId)
                .Select(p => p.Id)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<int>> GetPropertyIdsBySaleTypeAsync(int saleTypeId)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.SaleTypeId == saleTypeId)
                .Select(p => p.Id)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Property>> GetPropertiesByPropertyTypeWithImagesAsync(int propertyTypeId)
        {
            return await _context.Properties
                .AsNoTracking()
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
                .Where(p => p.PropertyTypeId == propertyTypeId)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Property>> GetPropertiesBySaleTypeWithImagesAsync(int saleTypeId)
        {
            return await _context.Properties
                .AsNoTracking()
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
                .Where(p => p.SaleTypeId == saleTypeId)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Property>> GetPropertiesByAgentWithImagesAsync(string agentId)
        {
            return await _context.Properties
                .AsNoTracking()
                .Include(p => p.Images.OrderByDescending(img => img.CreateAt))
                .Where(p => p.AgentId == agentId)
                .ToListAsync();
        }

        public async Task<int> GetAvailablePropertiesCountAsync(PropertyFilterCriteria? criteria = null)
        {
            var query = _context.Properties
                .AsNoTracking()
                .Where(p => p.Status == PropertyState.Available);

            if (criteria != null)
            {
                if (criteria.PropertyTypeId.HasValue)
                {
                    query = query.Where(p => p.PropertyTypeId == criteria.PropertyTypeId.Value);
                }
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
            }

            return await query.CountAsync();
        }

        public async Task<int> GetPropertiesCountByAgentAsync(string agentId, PropertyState? status = null)
        {
            var query = _context.Properties
                .AsNoTracking()
                .Where(p => p.AgentId == agentId);

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            return await query.CountAsync();
        }
    }
}
