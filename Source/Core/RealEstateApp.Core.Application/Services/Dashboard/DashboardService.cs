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
        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApi _internalAccountApi;

        public DashboardService(IPropertyService propertyService, IOperationalAccountWebApi internalAccountApi)
        {
            _propertyService = propertyService;
            _internalAccountApi = internalAccountApi;
        }

        public async Task<ValidationResult<DashboardDto>> GetDashboardStatsAsync(bool showActive = true)
        {
            var propertyTotals = await _propertyService.GetTotalsByStatusAsync();
            if (!propertyTotals.IsValid || propertyTotals.Value == null)
            {
                return ValidationResult<DashboardDto>.Failure(
                    propertyTotals.Errors.Count > 0 
                        ? propertyTotals.Errors.ToArray() 
                        : new[] { new Error("Oops", "No fue posible obtener las estadísticas de propiedades.") });
            }

            var agentsCount = await _internalAccountApi.GetUserAgentActiverOrInactive(showActive);
            var devsCount = await _internalAccountApi.GetUserDevelopersActiveOrInactive(showActive);
            var clientsCount = await _internalAccountApi.GetUserClientAciveOrInactive(showActive);

            return ValidationResult<DashboardDto>.Success(new DashboardDto
            {
                AvailableProperties = propertyTotals.Value.AvailableProperties,
                SoldProperties = propertyTotals.Value.SoldProperties,
                AgentsCount = agentsCount,
                ClientsCount = clientsCount,
                DevelopersCount = devsCount,
                ShowingActive = showActive
            });
        }
    }
}
