using RealEstateApp.Core.Application.Common.Errors;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Application.Contracts.Users;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;

using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services
{
    // Servicio para la pantalla de mantenimiento de desarrolladores
    public sealed class DeveloperService : IDeveloperService
    {
        private readonly IOperationalAccountWebApp _accountWebApp;

        public DeveloperService(IOperationalAccountWebApp accountWebApp)
        {
            _accountWebApp = accountWebApp;
        }

        public async Task<ValidationResult<IReadOnlyCollection<GetInternalUserDto>>> GetDevelopersAsync()
        {
            var result = await _accountWebApp.GetInternalUserGetAll(Roles.Desarrollador);
            return ValidationResult<IReadOnlyCollection<GetInternalUserDto>>.Success(result);
        }

        public async Task<ValidationResult> CreateAsync(RegisterInternalUsersDto dto)
        {
            // PENDIENTE DE CONFIRMAR: RegisterInternalUserAsync() en IOperationalAccountWebApp de Joel
            return await Task.FromResult(
                ValidationResult.Failure(
                    ErrorPendingIntegration.DeveloperCreate
                )
            );
        }

        public async Task<ValidationResult> EditAsync(EditInernalUserDto dto)
        {
            // PENDIENTE DE CONFIRMAR: EditInternalUserAsync() en IOperationalAccountWebApp de Joel
            return await Task.FromResult(
                ValidationResult.Failure(
                    ErrorPendingIntegration.DeveloperEdit
                )
            );
        }

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto)
        {
            // PENDIENTE DE CONFIRMAR: ChangeUserStatusAsync() en IOperationalAccountWebApp de Joel
            // (ErrorDeveloper.Activated / ErrorDeveloper.Inactivated se usarán en el controlador
            //  como mensajes de éxito tras la respuesta real de Joel)
            return await Task.FromResult(
                ValidationResult.Failure(
                    ErrorPendingIntegration.DeveloperToggle
                )
            );
        }
    }
}
