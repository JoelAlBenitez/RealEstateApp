using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;

using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.Contracts.Users.Base;
using RealEstateApp.Core.Application.Contracts.Developer;

namespace RealEstateApp.Core.Application.Services.Developer
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

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto)
        {
            var result = await _internalAccountApi.ChangeStateAsync(dto);
            if (result.HasError)
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            return ValidationResult.Success();
        }
    }
}
