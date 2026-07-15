using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Agent;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Common.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services.Agent
{
    // Servicio para la pantalla de listado de agentes
    public sealed class AgentManagementService : IAgentManagementService
    {
        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApi _internalAccountApi;

        public AgentManagementService(IPropertyService propertyService, IOperationalAccountWebApi internalAccountApi)
        {
            _propertyService = propertyService;
            _internalAccountApi = internalAccountApi;
        }

        public async Task<ValidationResult<IReadOnlyCollection<AdminConsultAgentDto>>> GetAgentsAsync()
        {
            var agents = await _internalAccountApi.GetAllAgentesByConsultAdmin();
            foreach (var agent in agents)
            {
                var countResult = await _propertyService.CountByAgentAsync(agent.Id);
                if (countResult.IsValid)
                {
                    agent.Properties = countResult.Value;
                }
            }
            return ValidationResult<IReadOnlyCollection<AdminConsultAgentDto>>.Success(agents);
        }

        public async Task<ValidationResult> ToggleStatusAsync(AlterStateUserDto dto)
        {
            var result = await _internalAccountApi.ChangeStateAsync(dto);
            if (result.HasError)
            {
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            }
            return ValidationResult.Success();
        }

        public async Task<ValidationResult> DeleteAgentAsync(string agentId)
        {
            // Paso 0 — Validar existencia y rol del agente (PENDIENTE DE CONFIRMAR CON JOEL):
            // El documento funcional exige verificar que el agente exista y tenga rol Agente antes
            // de ejecutar cualquier purga. Requiere un método de consulta en IOperationalAccountWebApi
            // (ej. GetUserByIdAsync o similar) que todavía no existe.
            // Si el agente no existe → retornar ValidationResult.Failure(ErrorAgent.NotFound)
            // Si el usuario existe pero no tiene rol Agente → retornar un error de rol incorrecto.
            // NO ejecutar el Paso 1 sin resolver este paso primero.
            var agent = await _internalAccountApi.GetUserBaseById(agentId);
            if (agent == null)
            {
                return ValidationResult.Failure(ErrorAgent.NotFound);
            }

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
            var result = await _internalAccountApi.DeleteAsync(agentId);
            if (result.HasError)
            {
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            }

            return ValidationResult.Success();
        }
    }
}
