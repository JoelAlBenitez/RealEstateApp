using AutoMapper;
using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;

namespace RealEstateApp.Core.Application.Services.Generic
{
    public abstract class GenericServices<TDtoModel, TEntity, TKey> :
        IGenericServices<TDtoModel, TKey>
        where TEntity : class 
        where TDtoModel : class
    {

        protected readonly IGenericRepository<TEntity, TKey> _genericRepository;
        protected readonly IMapper _mapper;
        protected readonly List<Error> _errors = new List<Error>();

        public GenericServices(IGenericRepository<TEntity, TKey> genericRepository,
            IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public virtual async Task<ValidationResult> AddAsync(TDtoModel dto)
        {
            try
            {
                var mapEntity = _mapper.Map<TEntity>(dto);
                await _genericRepository.AddAsync(mapEntity);
                var result = await _genericRepository.SaveAsync();
                if(result > 0)
                    return ValidationResult.Success();

                _errors.Add(new Error("Error",
                    "Ocurrió un error al procesar la solicitud. Intente nuevamente más tarde."));
                return ValidationResult.Failure(_errors);
            }
            catch (Exception) {
                _errors.Add(new Error("Error",
                       "Esta función no se encuentra disponible en este momento. Intente nuevamente más tarde."));
                return ValidationResult.Failure(_errors);
            }
        }

        public virtual async Task<ValidationResult<IReadOnlyCollection<TDtoModel>>> GetAllAsync()
        {
            try
            {
                var result = await _genericRepository.GetAllAsync();
                var mapResult = _mapper.Map<IReadOnlyCollection<TDtoModel>>(result);
                return ValidationResult<IReadOnlyCollection<TDtoModel>>.Success(mapResult);
            }
            catch (Exception) {

                _errors.Add(new Error("Error",
                   "Esta función no se encuentra disponible en este momento. Intente nuevamente más tarde."));
                return ValidationResult<IReadOnlyCollection<TDtoModel>>.Failure(_errors);
            }
        }

        public virtual async Task<ValidationResult<TDtoModel>> GetByIdAsync(TKey id)
        {
            try
            {
                var result = await _genericRepository.GetByIdAsync(id);
                var mapResult = _mapper.Map<TDtoModel>(result);
                return ValidationResult<TDtoModel>.Success(mapResult);
            }
            catch (Exception) {

                _errors.Add(new Error("Error",
                    "Esta opción no se encuentra disponible en este momento. Intente nuevamente más tarde."));
                return ValidationResult<TDtoModel>.Failure(_errors);
            }
        }

        public virtual async Task<ValidationResult> RemoveAsync(TKey id)
        {
            try
            {
                var entity = await _genericRepository.GetByIdAsync(id);
                if(entity == null)
                {
                    _errors.Add(new Error("Error",
                        "El elemento seleccionado ya no se encuentra disponible. Verifique e intente de nuevo."
                        ));
                    return ValidationResult.Failure(_errors);
                }
                var resut = await _genericRepository.DeleteAsync(entity);
                if (!resut)
                {
                    _errors.Add(new Error("Error",
                        "Ocurrió un error al eliminar el elemento. Intente nuevamente más tarde."));
                    return ValidationResult.Failure(_errors);
                }

                return ValidationResult.Success();
            }
            catch (Exception) {

                _errors.Add(new Error("Error",
                    "Esta función no se encuentra disponible en este momento. Intente nuevamente más tarde."));
                return ValidationResult.Failure(_errors);
            }
        }

        public virtual async Task<ValidationResult?> UpdateAsync(TDtoModel dto)
        {
            try
            {
                var mapEntity = _mapper.Map<TEntity>(dto);
                var result = await _genericRepository.UpdateAsync(mapEntity);
                if (!result)
                {
                    _errors.Add(new Error("Error",
                        "Ocurrió un error al actualizar el elemento. Intente nuevamente más tarde."));
                    return ValidationResult.Failure(_errors);
                }

                return ValidationResult.Success();

            }
            catch (Exception)
            {
                _errors.Add(new Error("Error",
                   "Esta función no se encuentra disponible en este momento. Intente nuevamente más tarde."));
                return ValidationResult.Failure(_errors);

            }
        }
    }
}
