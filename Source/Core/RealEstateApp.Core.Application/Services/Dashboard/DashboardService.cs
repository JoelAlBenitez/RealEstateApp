using RealEstateApp.Core.Application.Common.Errors;
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
            if (!propertyTotals.IsValid)
                return ValidationResult<DashboardDto>.Failure(propertyTotals.Errors.ToArray());

            // TODO: IOperationalAccountWebApi todavía no tiene estos 3 métodos mergeados a development 
            // (están en un PR sin aprobar de Joel).
            // Firmas exactas esperadas:
            // Task<int> GetUserAgentActiverOrInactive(bool isActive = true);
            // Task<int> GetUserDevelopersActiveOrInactive(bool isActive = true);
            // Task<int> GetUserClientAciveOrInactive(bool isActive = true);
            
            return await Task.FromResult(
                ValidationResult<DashboardDto>.Failure(ErrorPendingIntegration.DashboardStats)
            );
        }
    }
}
