using RealEstateApp.Core.Application.Contracts.Admin;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services.Admin
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
            try
            {
                var devs = await _internalAccountApi.GetAllInternalUsersByRol(Roles.Desarrollador);
                return ValidationResult<IReadOnlyCollection<GetInternalUserDto>>.Success(devs);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<GetInternalUserDto>>.Failure(
                    new Error("Query_Error", "Al parecer esta función no está disponible en este momento. Favor intente de nuevo más tarde.")
                );
            }
        }

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto)
        {
            try
            {
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
