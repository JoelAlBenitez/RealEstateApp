using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.DTOs.PropertyType;

using RealEstateApp.Core.Application.DTOs.Property;

namespace RealEstateApp.Core.Application.Contracts.PropertyType
{
    // Trazabilidad: Corresponde a la pantalla "Mantenimiento de tipo de propiedades"
    // del documento funcional.
    public interface IPropertyTypeService : IGenericServices<SavePropertyTypeDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<PropertyTypeDto>>> GetAllWithCountAsync();
        
        // Creado para el dropdown de Property de Joel
        Task<ValidationResult<IReadOnlyCollection<TypeProperty>>> GetAllForSelectAsync();
    }
}
