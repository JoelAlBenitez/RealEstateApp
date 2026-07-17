using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Generic;


namespace RealEstateApp.Infraestructure.Persistence.Repositories.PropertyTypeRepo
{
    public sealed class PropertyTypeRepository : GenericRepository<PropertyType, int>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(DbContextRealEstateApp context) : base(context)
        {
        }

        public async Task<int> CountByPropertyTypeAsync(int propertyTypeId)
        {
            // TODO: Implementar la consulta real cuando la relación esté mapeada.
            return await Task.FromResult(0);
        }

        public async Task<bool> DeleteWithAssociatedPropertiesAsync(PropertyType propertyType, IReadOnlyCollection<int> propertyIds)
        {
            // TODO: Implementar la lógica real de borrado en cascada (acuerdo de Sebastián).
            return await Task.FromResult(true);
        }

        public async Task<bool> ExistNameAsync(string name, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var lowerName = name.Trim().ToLower();

            // Si id == 0 (Create), busca cualquier coincidencia.
            // Si id != 0 (Update), busca cualquier coincidencia en OTROS registros.
            return await _context.Set<PropertyType>()
                .AnyAsync(e => e.Name.ToLower() == lowerName && e.Id != id);
        }
    }
}
