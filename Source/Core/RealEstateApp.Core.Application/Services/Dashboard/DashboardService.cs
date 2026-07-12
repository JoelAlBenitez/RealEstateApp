using RealEstateApp.Core.Application.Common.Errors;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Dashboard;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Dashboard;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services.Dashboard
{
    // Servicio para la pantalla de indicadores (Home del Administrador)
    public sealed class DashboardService : IDashboardService
    {
        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApi _internalAccountApi;

        public DashboardService(IPropertyService propertyService, IOperationalAccountWebApi internalAccountApi)
        {
            _propertyService = propertyService;
            _internalAccountApi = internalAccountApi;
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
                
                // PENDIENTE: Joel no ha expuesto GetUserCountersAsync() en IOperationalAccountWebApi todavía
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
