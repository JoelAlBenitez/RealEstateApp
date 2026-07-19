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
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;

        public ImprovementRepository(DbContextRealEstateApp context, IPropertyImprovementRepository propertyImprovementRepository) : base(context)
        {
            _propertyImprovementRepository = propertyImprovementRepository;
        }

        public async Task<int> CountByImprovementAsync(int improvementId)
        {
            var allAssociations = await _propertyImprovementRepository.GetAllAsync();
            return allAssociations.Count(pi => pi.ImprovementId == improvementId);
        }

        public async Task<bool> ExistNameAsync(string name, int id = 0)
        {
            var lowerName = name.Trim().ToLower();

            return await _context.Set<Improvement>()
                .AnyAsync(e => e.Name.ToLower() == lowerName && e.Id != id);
        }
    }
}
