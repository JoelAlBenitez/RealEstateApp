using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infraestructure.Persistence.Repositories.PropertyImprovements
{
    public sealed class PropertyImprovementRepository : GenericRepository<PropertyImprovement, int>, IPropertyImprovementRepository
    {
        public PropertyImprovementRepository(DbContextRealEstateApp context) : base(context) { }
    }
}
