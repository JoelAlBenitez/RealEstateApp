using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;
namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IPropertyTypeRepository : IGenericRepository<PropertyType, int>
    {
        Task<int> CountByPropertyTypeAsync(int propertyTypeId);
        // TODO PENDIENTE DE CONFIRMAR: estrategia de cascada al eliminar (Fluent API
        // OnDelete:Cascade vs método explícito) — no implementar sin confirmar con el equipo.
    }
}
