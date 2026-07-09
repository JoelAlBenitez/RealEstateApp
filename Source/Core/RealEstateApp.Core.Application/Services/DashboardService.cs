using RealEstateApp.Core.Application.Common.Errors;
using RealEstateApp.Core.Application.Contracts.Dashboard;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users;
using RealEstateApp.Core.Application.DTOs.Dashboard;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services
{
    // Servicio para la pantalla de indicadores (Home del Administrador)
    public sealed class DashboardService : IDashboardService
    {
        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApp _accountWebApp;

        public DashboardService(IPropertyService propertyService, IOperationalAccountWebApp accountWebApp)
        {
            _propertyService = propertyService;
            _accountWebApp = accountWebApp;
        }

        public async Task<ValidationResult<DashboardDto>> GetDashboardStatsAsync()
        {
            // PENDIENTE DE CONFIRMAR CON JOEL: GetUserCountersAsync() → agentes/clientes/desarrolladores activos e inactivos
            // PENDIENTE DE CONFIRMAR CON SEBASTIÁN: GetTotalsByStatusAsync() → propiedades disponibles y vendidas
            return await Task.FromResult(
                ValidationResult<DashboardDto>.Failure(
                    ErrorPendingIntegration.DashboardStats
                )
            );
        }
    }
}
