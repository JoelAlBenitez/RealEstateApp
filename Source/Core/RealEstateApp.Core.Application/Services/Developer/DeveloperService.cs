using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Application.Contracts.Users;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;

using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.Contracts.Users.Base;

namespace RealEstateApp.Core.Application.Services.Developer
{
    // Servicio para la pantalla de mantenimiento de desarrolladores
    public sealed class DeveloperService : IDeveloperService
    {
        private readonly IOperationalAccountWebApi _internalAccountApi;
        private readonly IBaseAccountUser _baseAccount;

        public DeveloperService(IOperationalAccountWebApi internalAccountApi, IBaseAccountUser baseAccount)
        {
            _internalAccountApi = internalAccountApi;
            _baseAccount = baseAccount;
        }

        public async Task<ValidationResult<IReadOnlyCollection<GetInternalUserDto>>> GetDevelopersAsync()
        {
            var result = await _internalAccountApi.GetAllInternalUsersByRol(Roles.Desarrollador);
            return ValidationResult<IReadOnlyCollection<GetInternalUserDto>>.Success(result);
        }

        public async Task<ValidationResult> CreateAsync(RegisterInternalUsersDto dto)
        {
            // PENDIENTE DE CONFIRMAR: RegisterInternalUserAsync() en IOperationalAccountWebApi de Joel
            // TODO: reemplazar por ErrorPendingIntegration cuando llegue via merge de AdminBase
            return await Task.FromResult(
                ValidationResult.Failure(new Error("Dev_Pending", "Pendiente de Joel")));
        }

        public async Task<ValidationResult> EditAsync(EditInternalUserDto dto)
        {
            // PENDIENTE DE CONFIRMAR: EditInternalUserAsync() en IOperationalAccountWebApi de Joel
            // TODO: reemplazar por ErrorPendingIntegration cuando llegue via merge de AdminBase
            return await Task.FromResult(
                ValidationResult.Failure(new Error("Dev_Pending", "Pendiente de Joel")));
        }

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto)
        {
            var result = await _baseAccount.ChangeStateAsync(dto);
            if (result.HasError)
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            return ValidationResult.Success();
        }
    }
}
