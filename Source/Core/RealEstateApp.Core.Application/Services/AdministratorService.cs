using RealEstateApp.Core.Application.Common.Errors;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;

using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.Contracts.Users;
using RealEstateApp.Core.Application.Contracts.Users.Base;

namespace RealEstateApp.Core.Application.Services
{
    // Servicio para la pantalla de mantenimiento de administradores
    public sealed class AdministratorService : IAdministratorService
    {
        private readonly IOperationalAccountWebApi _internalAccountApi;
        // IBaseAccountUser aún no está registrado en el contenedor de DI (RegistrationAndConfigurationsIdentity.cs sigue vacío) — compilará pero no funcionará en runtime hasta que Joel complete ese registro.
        private readonly IBaseAccountUser _baseAccount;

        public AdministratorService(IOperationalAccountWebApi internalAccountApi, IBaseAccountUser baseAccount)
        {
            _internalAccountApi = internalAccountApi;
            _baseAccount = baseAccount;
        }

        public async Task<ValidationResult<IReadOnlyCollection<GetInternalUserDto>>> GetAdministratorsAsync()
        {
            var admins = await _internalAccountApi.GetAllInternalUsersByRol(Roles.Administrador);
            return ValidationResult<IReadOnlyCollection<GetInternalUserDto>>.Success(admins);
        }

        public async Task<ValidationResult> CreateAsync(RegisterInternalUsersDto dto)
        {
            // PENDIENTE DE CONFIRMAR: RegisterInternalUserAsync() en IOperationalAccountWebApi de Joel
            return await Task.FromResult(
                ValidationResult.Failure(
                    ErrorPendingIntegration.AdminCreate
                )
            );
        }

        public async Task<ValidationResult> EditAsync(EditInternalUserDto dto, string currentAdminId)
        {
            // Regla de auto-protección: no se puede editar a sí mismo
            if (dto.Id == currentAdminId)
            {
                return ValidationResult.Failure(
                    ErrorAdministrator.SelfEdit
                );
            }

            // PENDIENTE DE CONFIRMAR: validar existencia del administrador vía
            // IOperationalAccountWebApi antes de editar. Si el administrador no existe,
            // retornar ValidationResult.Failure(ErrorAdministrator.NotFound).
            // Como el método de Joel para verificar esto todavía no existe, se deja como placeholder.

            // PENDIENTE DE CONFIRMAR: EditInternalUserAsync() en IOperationalAccountWebApi de Joel
            return await Task.FromResult(
                ValidationResult.Failure(
                    ErrorPendingIntegration.AdminEdit
                )
            );
        }

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto, string currentAdminId)
        {
            // Regla de auto-protección: no se puede inactivar a sí mismo
            // Auto-inactivación: se valida localmente comparando IDs porque no depende de ningún
            // dato externo — solo necesitamos saber si el admin está intentando inactivarse a sí mismo.
            // La regla de mínimo-un-admin-activo NO se valida aquí porque requiere GetUserCountersAsync()
            // de Joel para conocer el conteo actual de admins activos, y ese método aún no existe.
            if (dto.Id == currentAdminId && !dto.State)
            {
                return ValidationResult.Failure(
                    ErrorAdministrator.SelfInactivation
                );
            }

            // PENDIENTE DE CONFIRMAR: validar existencia del administrador vía
            // IOperationalAccountWebApi antes de cambiar estado. Si el administrador no existe,
            // retornar ValidationResult.Failure(ErrorAdministrator.NotFound).
            // Como el método de Joel para verificar esto todavía no existe, se deja como placeholder.

            var result = await _baseAccount.ChangeStateAsync(dto);
            
            if (result.HasError)
            {
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            }

            return ValidationResult.Success();
        }
    }
}
