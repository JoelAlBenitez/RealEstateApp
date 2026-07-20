using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Properties
{
    public interface IPropertyApiQueryService
    {
        Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllForApiAsync();
        Task<ValidationResult<PropertyDto>> GetByCodeForApiAsync(string code);
    }
}
