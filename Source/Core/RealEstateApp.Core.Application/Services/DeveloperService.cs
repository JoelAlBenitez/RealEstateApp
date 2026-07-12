using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Application.Contracts.Users;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;

using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services
{
    // Servicio para la pantalla de mantenimiento de desarrolladores
    public sealed class DeveloperService : IDeveloperService
    {
        private readonly IOperationalAccountWebApi _internalAccountApi;

        public DeveloperService(IOperationalAccountWebApi internalAccountApi)
        {
            _internalAccountApi = internalAccountApi;
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
            // PENDIENTE DE CONFIRMAR: ChangeUserStatusAsync() en IOperationalAccountWebApi de Joel
            // (ErrorDeveloper.Activated / ErrorDeveloper.Inactivated se usarán en el controlador
            //  como mensajes de éxito tras la respuesta real de Joel)
            // TODO: reemplazar por ErrorPendingIntegration cuando llegue via merge de AdminBase
            return await Task.FromResult(
                ValidationResult.Failure(new Error("Dev_Pending", "Pendiente de Joel")));
        }
    }
}
