using System.Threading.Tasks;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Improvement
{
    public interface IImprovementValidationService
    {
        Task<ValidationResult> ValidateForCreateAsync(SaveImprovementDto dto);
        Task<ValidationResult> ValidateForUpdateAsync(SaveImprovementDto dto);
    }
}
