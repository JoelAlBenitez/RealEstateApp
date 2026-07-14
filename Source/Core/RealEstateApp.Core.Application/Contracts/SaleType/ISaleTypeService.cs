using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.SaleType;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.SaleType
{
    // Trazabilidad: Corresponde a la pantalla "Mantenimiento de tipo de ventas"
    // del documento funcional.
    public interface ISaleTypeService : IGenericServices<SaveSaleTypeDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<SaleTypeDto>>> GetAllWithCountAsync();
    }
}
