using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Agent;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Common.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

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
            // NOTA: TransactionScope requiere que el proveedor de base de datos soporte
            // transacciones distribuidas/ambient. Mientras el proyecto use
            // UseInMemoryDatabase (configuración actual confirmada en
            // InfraestructurePersistenceDependencies.cs), esta transacción puede no
            // comportarse igual que con SQL Server real — verificar al migrar a
            // producción con SQL Server.
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);

            // Paso 0 — Validar existencia del agente: GetUserBaseById ya está implementado y funcionando.
            // La validación de ROL específico (no solo existencia) queda pendiente de un método adicional de Joel si se requiere en el futuro.
            var agent = await _internalAccountApi.GetUserBaseById(agentId);
            if (agent == null)
            {
                return ValidationResult.Failure(ErrorAgent.NotFound);
            }

            // Paso 1 — Cascada de propiedades:
            // DeletePropertiesByAgentAsync ya existe en IPropertyService, implementado por Sebastián, y está siendo consumido correctamente aquí.
            var cascadeResult = await _propertyService.DeletePropertiesByAgentAsync(agentId);
            if (!cascadeResult.IsValid)
            {
                return cascadeResult; // el 'using' libera el scope sin scope.Complete() = rollback automático
            }

            // Paso 2 — Eliminación del usuario (Joel):
            // se usa TransactionScope con ReadCommitted; funciona correctamente con SQL Server, pero mientras el proyecto use InMemoryDatabase (configuración actual), el comportamiento transaccional puede no ser 100% equivalente — revisar al migrar a SQL Server real.
            var result = await _internalAccountApi.DeleteAsync(agentId);
            if (result.HasError)
            {
                return ValidationResult.Failure(new Error("Identity_Error", string.Join(", ", result.Errors ?? new List<string>())));
            }

            scope.Complete(); // confirma la transacción completa solo si todo fue exitoso
            return ValidationResult.Success();
        }
    }
}
