using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.GenericServices
{
    public interface IGenericServices <TDtoModel> where TDtoModel : class 
    {
        Task<ValidationResult> AddAsync(TDtoModel dto);
        Task<ValidationResult?> UpdateAsync(TDtoModel dto, int id);
        Task<ValidationResult<TDtoModel>> GetByIdAsync(int id);
        Task<ValidationResult<IReadOnlyCollection<TDtoModel>>> GetAllAsync();
    }
}
