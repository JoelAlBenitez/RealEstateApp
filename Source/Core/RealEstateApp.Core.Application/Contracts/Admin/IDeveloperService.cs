using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Admin
{
    // Trazabilidad: Corresponde a la pantalla "Mantenimiento de desarrolladores" del documento funcional.
    public interface IDeveloperService
    {
        // Obtiene el listado de desarrolladores registrados en el sistema
        Task<ValidationResult<IReadOnlyCollection<GetInternalUserDto>>> GetDevelopersAsync();

        // Cambia el estado de un desarrollador (Activo/Inactivo)
        Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto);
    }
}
