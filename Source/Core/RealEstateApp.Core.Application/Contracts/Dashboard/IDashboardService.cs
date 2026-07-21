using RealEstateApp.Core.Application.DTOs.Dashboard;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Contracts.Dashboard
{
    
    // Trazabilidad: Corresponde a la pantalla "Home del administrador" (indicadores) del documenDashboardServiceto funcional.
    
    public interface IDashboardService
    {
        Task<ValidationResult<DashboardDto>> GetDashboardStatsAsync();
    }
}
