using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Improvement
{
    // Trazabilidad: Corresponde a la pantalla "Mantenimiento de mejoras" del documento funcional.
    public interface IImprovementService : IGenericServices<SaveImprovementDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<ImprovementDto>>> GetAllWithCountAsync();
    }
}
