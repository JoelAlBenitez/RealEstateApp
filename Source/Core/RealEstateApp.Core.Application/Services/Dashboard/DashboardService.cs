using RealEstateApp.Core.Application.Common.Errors;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Dashboard;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Dashboard;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services.Dashboard
{
    // Servicio para la pantalla de indicadores (Home del Administrador)
    public sealed class DashboardService : IDashboardService
    {
        private readonly IPropertyCascadeService _propertyService;
        private readonly IOperationalAccountWebApi _internalAccountApi;

        public DashboardService(IPropertyCascadeService propertyService, IOperationalAccountWebApi internalAccountApi)
        {
            _propertyService = propertyService;
            _internalAccountApi = internalAccountApi;
        }

        public async Task<ValidationResult<DashboardDto>> GetDashboardStatsAsync()
        {
            var propertyTotals = await _propertyService.GetTotalsByStatusAsync();
            if (!propertyTotals.IsValid || propertyTotals.Value == null)
            {
                return ValidationResult<DashboardDto>.Failure(
                    propertyTotals.Errors.Count > 0 
                        ? propertyTotals.Errors.ToArray() 
                        : new[] { new Error("Oops", "No fue posible obtener las estadísticas de propiedades.") });
            }

            var activeAgents = await _internalAccountApi.GetUserAgentActiverOrInactive(true);
            var inactiveAgents = await _internalAccountApi.GetUserAgentActiverOrInactive(false);
            
            var activeDevs = await _internalAccountApi.GetUserDevelopersActiveOrInactive(true);
            var inactiveDevs = await _internalAccountApi.GetUserDevelopersActiveOrInactive(false);
            
            var activeClients = await _internalAccountApi.GetUserClientAciveOrInactive(true);
            var inactiveClients = await _internalAccountApi.GetUserClientAciveOrInactive(false);

            return ValidationResult<DashboardDto>.Success(new DashboardDto
            {
                AvailableProperties = propertyTotals.Value.AvailableProperties,
                SoldProperties = propertyTotals.Value.SoldProperties,
                ActiveAgentsCount = activeAgents,
                InactiveAgentsCount = inactiveAgents,
                ActiveClientsCount = activeClients,
                InactiveClientsCount = inactiveClients,
                ActiveDevelopersCount = activeDevs,
                InactiveDevelopersCount = inactiveDevs
            });
        }
    }
}
