using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.GenericServices
{
    public interface IGenericServices <TDtoModel, TKey> where TDtoModel : class 
    {
        Task<ValidationResult> AddAsync(TDtoModel dto);
        Task<ValidationResult?> UpdateAsync(TDtoModel dto, TKey id);
        Task<ValidationResult<TDtoModel>> GetByIdAsync(TKey id);
        Task<ValidationResult> RemoveAsync(TKey id);
        Task<ValidationResult<IReadOnlyCollection<TDtoModel>>> GetAllAsync();
        Task<ValidationResult> DeleteAsync(TKey id);
    }
}
