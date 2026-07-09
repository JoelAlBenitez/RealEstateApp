using RealEstateApp.Core.Application.Common.Errors;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services
{
    // Servicio para la pantalla de listado de agentes
    public sealed class AgentManagementService : IAgentManagementService
    {
        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApp _accountWebApp;

        public AgentManagementService(IPropertyService propertyService, IOperationalAccountWebApp accountWebApp)
        {
            _propertyService = propertyService;
            _accountWebApp = accountWebApp;
        }

        public async Task<ValidationResult<IReadOnlyCollection<AdminConsultAgentDto>>> GetAgentsAsync()
        {
            var result = await _accountWebApp.GetAgentByConsultAdminAll();
            return ValidationResult<IReadOnlyCollection<AdminConsultAgentDto>>.Success(result);
        }

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto)
        {
            // PENDIENTE DE CONFIRMAR: ChangeUserStatusAsync() en IOperationalAccountWebApp de Joel
            // (ErrorAgent.ConfirmInactivate / ErrorAgent.ConfirmActivate se usan en la UI/controlador
            //  como mensajes de confirmación antes de llamar a este método)
            return await Task.FromResult(
                ValidationResult.Failure(
                    ErrorPendingIntegration.AgentToggle
                )
            );
        }

        public async Task<ValidationResult> DeleteAgentAsync(string agentId)
        {
            // Paso 0 — Validar existencia y rol del agente (PENDIENTE DE CONFIRMAR CON JOEL):
            // El documento funcional exige verificar que el agente exista y tenga rol Agente antes
            // de ejecutar cualquier purga. Requiere un método de consulta en IOperationalAccountWebApp
            // (ej. GetUserByIdAsync o similar) que todavía no existe.
            // Si el agente no existe → retornar ValidationResult.Failure(ErrorAgent.NotFound)
            // Si el usuario existe pero no tiene rol Agente → retornar un error de rol incorrecto.
            // NO ejecutar el Paso 1 sin resolver este paso primero.

            // Paso 1 — Cascada de propiedades (Sebastián):
            // Nombre de método confirmado en distribucion-equipo.html. Encapsula la eliminación
            // completa: propiedades + imágenes + mejoras + ofertas + mensajes + favoritos del agente.
            // PENDIENTE DE CONFIRMAR CON SEBASTIÁN: tipo de retorno y firma final contra el código real.
            var cascadeResult = await _propertyService.DeletePropertiesByAgentAsync(agentId);
            if (!cascadeResult.IsValid)
            {
                return cascadeResult;
            }

            // Paso 2 — Eliminación del usuario (Joel):
            // PENDIENTE DE CONFIRMAR CON JOEL: esta secuencia (purgar propiedades + eliminar usuario)
            // requiere una transacción explícita (BeginTransactionAsync/CommitAsync/RollbackAsync) que
            // envuelva ambas operaciones, según la especificación de persistencia e Identity.
            // NO se implementa la transacción todavía porque no se ha confirmado cómo Joel expone el DbContext.

            // PENDIENTE DE CONFIRMAR CON JOEL: DeleteUserAsync() en IOperationalAccountWebApp de Joel
            return await Task.FromResult(
                ValidationResult.Failure(
                    ErrorPendingIntegration.AgentDelete
                )
            );
        }
    }
}
