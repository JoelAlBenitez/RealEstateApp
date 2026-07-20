using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Improvement
{
    public interface IImprovementService : IGenericServices<SaveImprovementDto, int>
    {
        Task<ValidationResult<IReadOnlyCollection<ImprovementDto>>> GetAllWithCountAsync();
        Task<ValidationResult<IReadOnlyCollection<TypeImprovement>>> GetAllForSelectAsync();
    }
}
