using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;

using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Admin
{
    
    // Trazabilidad: Corresponde a la pantalla "Mantenimiento de administradores" del documento funcional.
   
    public interface IAdministratorService
    {
        // Obtiene el listado de administradores registrados en el sistema
        Task<ValidationResult<IReadOnlyCollection<GetInternalUserDto>>> GetAdministratorsAsync();

        // Cambia el estado de un administrador, aplicando validación de auto-inactivación y mínimo un administrador activo
        Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto, string currentAdminId);
    }
}
