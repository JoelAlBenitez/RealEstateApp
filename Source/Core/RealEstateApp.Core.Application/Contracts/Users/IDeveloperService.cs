using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;

using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Users
{
    /// <summary>
    /// Trazabilidad: Corresponde a la pantalla "Mantenimiento de desarrolladores" del documento funcional.
    /// </summary>
    public interface IDeveloperService
    {
        // Obtiene el listado de desarrolladores registrados en el sistema
        Task<ValidationResult<IReadOnlyCollection<GetInternalUserDto>>> GetDevelopersAsync();

        // Registra un nuevo desarrollador
        Task<ValidationResult> CreateAsync(RegisterInternalUsersDto dto);

        // Edita un desarrollador existente
        Task<ValidationResult> EditAsync(EditInternalUserDto dto);

        // Cambia el estado de un desarrollador
        Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto);
    }
}
