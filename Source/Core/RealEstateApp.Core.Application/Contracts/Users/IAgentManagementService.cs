using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Users
{
    
    // Trazabilidad: Corresponde a la pantalla "Listado de los agentes" del documento funcional.
  
    public interface IAgentManagementService
    {
        // Obtiene el listado de agentes registrados
        Task<ValidationResult<IReadOnlyCollection<AdminConsultAgentDto>>> GetAgentsAsync();

        // Cambia el estado de un agente
        Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto);

        // Elimina a un agente en cascada física completa
        Task<ValidationResult> DeleteAgentAsync(string agentId);
    }
}
