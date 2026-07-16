using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Infraestructure.Persistence.Repositories.ImprovementRepo
{
    public sealed class ImprovementRepository : GenericRepository<Improvement, int>, IImprovementRepository
    {
        public ImprovementRepository(DbContextRealEstateApp context) : base(context)
        {
        }

        public async Task<int> CountByImprovementAsync(int improvementId)
        {
            // Nota: La relación N:M (PropertyImprovement) aún no está descomentada 
            // ni configurada en Property.cs o el DbContext.
            // PENDIENTE: Ajustar este método cuando Sebastián agregue la tabla puente.
            return await Task.FromResult(0);
        }
    }
}
