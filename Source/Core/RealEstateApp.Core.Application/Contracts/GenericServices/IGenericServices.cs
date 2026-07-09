using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.GenericServices
{
<<<<<<< HEAD
    public interface IGenericServices <TDtoModel> where TDtoModel : class 
    {
        Task<ValidationResult> AddAsync(TDtoModel dto);
        Task<ValidationResult?> UpdateAsync(TDtoModel dto, int id);
        Task<ValidationResult<TDtoModel>> GetByIdAsync(int id);
=======
    public interface IGenericServices <TDtoModel, TKey> where TDtoModel : class 
    {
        Task<ValidationResult> AddAsync(TDtoModel dto);
        Task<ValidationResult?> UpdateAsync(TDtoModel dto, TKey id);
        Task<ValidationResult<TDtoModel>> GetByIdAsync(TKey id);
>>>>>>> development
        Task<ValidationResult<IReadOnlyCollection<TDtoModel>>> GetAllAsync();
    }
}
