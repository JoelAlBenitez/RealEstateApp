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
            var admins = await _internalAccountApi.GetAllInternalUsersByRol(Roles.Administrador);
            return ValidationResult<IReadOnlyCollection<GetInternalUserDto>>.Success(admins);
        }

        public async Task<ValidationResult> CreateAsync(RegisterInternalUsersDto dto)
        {
            var result = await _internalAccountApi.CreateInternalUserAsync(dto);
            if (result.HasError)
            {
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            }
            return ValidationResult.Success();
        }

        public async Task<ValidationResult> EditAsync(EditInternalUserDto dto)
        {

            var result = await _internalAccountApi.UpdateInternalUserAsync(dto);
            if (result.HasError)
            {
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            }
            return ValidationResult.Success();
        }

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto, string currentAdminId)
        {
            var validationResult = _administratorValidationService.ValidateSelfInactivation(dto, currentAdminId);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var result = await _internalAccountApi.ChangeStateAsync(dto);
            
            if (result.HasError)
            {
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            }

            return ValidationResult.Success();
        }
    }
}
