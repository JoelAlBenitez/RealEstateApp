using RealEstateApp.Core.Application.Common.Errors;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using System.Collections.Generic;
using System.Threading.Tasks;

using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.Contracts.Admin;
using RealEstateApp.Core.Application.Contracts.Users.Base;

namespace RealEstateApp.Core.Application.Services.Admin
{
    // Servicio para la pantalla de mantenimiento de administradores
    public sealed class AdministratorService : IAdministratorService
    {
        private readonly IOperationalAccountWebApi _internalAccountApi;
        private readonly IAdministratorValidationService _administratorValidationService;

        public AdministratorService(IOperationalAccountWebApi internalAccountApi, IAdministratorValidationService administratorValidationService)
        {
            _internalAccountApi = internalAccountApi;
            _administratorValidationService = administratorValidationService;
        }

        public async Task<ValidationResult<IReadOnlyCollection<GetInternalUserDto>>> GetAdministratorsAsync()
        {
            try
            {
                var admins = await _internalAccountApi.GetAllInternalUsersByRol(Roles.Administrador);
                return ValidationResult<IReadOnlyCollection<GetInternalUserDto>>.Success(admins);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<GetInternalUserDto>>.Failure(
                    new Error("Query_Error", "Al parecer esta función no está disponible en este momento. Favor intente de nuevo más tarde.")
                );
            }
        }

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto, string currentAdminId)
        {
            try
            {
                var selfCheck = _administratorValidationService.ValidateSelfInactivation(dto, currentAdminId);
                if (!selfCheck.IsValid)
                {
                    return selfCheck;
                }

                var minCheck = await _administratorValidationService.ValidateMinimumActiveAdmin(dto);
                if (!minCheck.IsValid)
                {
                    return minCheck;
                }

                var result = await _internalAccountApi.ChangeStateAsync(dto);
                
                if (result.HasError)
                {
                    return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
                }

                return ValidationResult.Success();
            }
            catch (Exception)
            {
                return ValidationResult.Failure(
                    new Error("Toggle_Error", "Al parecer esta función no está disponible en este momento. Favor intente de nuevo más tarde.")
                );
            }
        }
    }
}
