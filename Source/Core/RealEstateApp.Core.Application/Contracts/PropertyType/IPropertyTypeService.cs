using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.DTOs.PropertyType;

namespace RealEstateApp.Core.Application.Contracts.PropertyType
{
    // Trazabilidad: Corresponde a la pantalla "Mantenimiento de tipo de propiedades"
    // del documento funcional.
    public interface IPropertyTypeService : IGenericServices<SavePropertyTypeDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<PropertyTypeDto>>> GetAllWithCountAsync();
    }
}
