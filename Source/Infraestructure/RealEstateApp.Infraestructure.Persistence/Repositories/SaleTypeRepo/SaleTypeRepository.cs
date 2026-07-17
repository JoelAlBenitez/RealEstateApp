using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Infraestructure.Persistence.Repositories.SaleTypeRepo
{
    public sealed class SaleTypeRepository : GenericRepository<SaleType, int>, ISaleTypeRepository
    {
        public SaleTypeRepository(DbContextRealEstateApp context) : base(context)
        {
        }

        public async Task<int> CountBySaleTypeAsync(int saleTypeId)
        {
            // TODO: Implementar la consulta real cuando la relación esté mapeada por Sebastián.
            return await Task.FromResult(0);
        }

        public async Task<bool> ExistNameAsync(string name, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var lowerName = name.Trim().ToLower();

            // Si id == 0 (Create), busca cualquier coincidencia.
            // Si id != 0 (Update), busca cualquier coincidencia en OTROS registros.
            return await _context.Set<SaleType>()
                .AnyAsync(e => e.Name.ToLower() == lowerName && e.Id != id);
        }
    }
}
