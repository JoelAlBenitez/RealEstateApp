using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;
namespace RealEstateApp.Core.Domain.Interfaces.Repositories
{
    public interface IPropertyTypeRepository : IGenericRepository<PropertyType, int>
    {
        Task<int> CountByPropertyTypeAsync(int propertyTypeId);
        
        // Elimina el PropertyType y las propiedades asociadas (recibidas como lista de IDs,
        // provista por Sebastián via un método que aún no existe). Marca cada eliminación
        // SIN guardar individualmente (Remove() sin SaveChangesAsync por cada una), y ejecuta
        // UN SOLO SaveChangesAsync() al final, fuera del bucle — si una eliminación falla,
        // ninguna se persiste (atomicidad). Instrucción literal de Joel (14/07).
        // PENDIENTE: falta que Sebastián exponga el método que retorna los IDs de propiedades
        // asociadas a un PropertyType (ej. GetPropertyIdsByTypeAsync(int typeId)).
        Task<bool> DeleteWithAssociatedPropertiesAsync(PropertyType propertyType, IReadOnlyCollection<int> associatedPropertyIds);
    }
}
