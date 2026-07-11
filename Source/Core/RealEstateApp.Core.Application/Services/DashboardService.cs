using RealEstateApp.Core.Application.Common.Errors;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Dashboard;
// TODO: descomentar cuando Joel suba su refactorización de IOperationalAccountWebApp
// using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Dashboard;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services
{
    // Servicio para la pantalla de indicadores (Home del Administrador)
    public sealed class DashboardService : IDashboardService
    {
        private readonly IPropertyService _propertyService;
        // TODO: descomentar cuando Joel suba su refactorización de IOperationalAccountWebApp
        // private readonly IOperationalAccountWebApp _accountWebApp;

        // TODO: descomentar cuando Joel suba su refactorización de IOperationalAccountWebApp
        // public DashboardService(IPropertyService propertyService, IOperationalAccountWebApp accountWebApp)
        public DashboardService(IPropertyService propertyService)
        {
            _propertyService = propertyService;
            // TODO: descomentar cuando Joel suba su refactorización de IOperationalAccountWebApp
            // _accountWebApp = accountWebApp;
        }

        public async Task<ValidationResult<DashboardDto>> GetDashboardStatsAsync()
        {
            var propertyTotals = await _propertyService.GetTotalsByStatusAsync();
            if (!propertyTotals.IsValid)
                return ValidationResult<DashboardDto>.Failure(propertyTotals.Errors.ToArray());

            var dto = new DashboardDto
            {
                AvailableProperties = propertyTotals.Value.AvailableProperties,
                SoldProperties = propertyTotals.Value.SoldProperties,
                
                // TODO: completar con GetUserCountersAsync() de Joel
                ActiveAgents = 0,
                InactiveAgents = 0,
                ActiveClients = 0,
                InactiveClients = 0,
                ActiveDevelopers = 0,
                InactiveDevelopers = 0
            };

            return ValidationResult<DashboardDto>.Success(dto);
        }
    }
}
